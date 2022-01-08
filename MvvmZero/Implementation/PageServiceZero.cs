/*
MIT License

Copyright(c) 2016 - 2022 Function Zero Ltd

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FunctionZero.MvvmZero
{
    public class PageServiceZero : IPageServiceZero
    {
        private readonly Func<INavigation> _navigationGetter;
        private Func<Type, object> _typeFactory;
        private Application _currentApplication;

        private INavigation CurrentNavigationPage => _navigationGetter();

        /// <summary>
        /// Creates a PageServiceZero associated with the provided NavigationPage.
        /// Uses a Func to get the INavigation for Push operations to allow
        /// multiple nav stacks when using a MasterDetail page or similar architecture.
        /// </summary>
        /// <param name="navPage"></param>
        /// <param name="typeFactory"></param>
        /// <param name="theApplicationInstance">Pass in App.Current if you want IHasOwnerPage wired up for you</param>
        public PageServiceZero(Func<INavigation> navigationGetter, Func<Type, object> typeFactory, Application currentApplication = null)
        {
            _navigationGetter = navigationGetter;
            _typeFactory = typeFactory;

            if (currentApplication == null)
                currentApplication = Application.Current;

            if (currentApplication != null)
            {
                Init(currentApplication);

            }
        }

        public GetMvvmPageMode DefaultMvvmPageMode { get; set; }

        public void Init(Application currentApplication)
        {
            if (_currentApplication != null)
            {
                _currentApplication.PageAppearing -= Current_PageAppearing;
                _currentApplication.PageDisappearing -= Current_PageDisappearing;
            }

            if (currentApplication != null)
            {
                currentApplication.PageAppearing += Current_PageAppearing;
                currentApplication.PageDisappearing += Current_PageDisappearing;
            }
            _currentApplication = currentApplication;
        }

        private void Current_PageAppearing(object sender, Page page)
        {
            if (page.BindingContext is IHasOwnerPage hop)
                hop.OnOwnerPageAppearing();
        }

        private void Current_PageDisappearing(object sender, Page page)
        {
            if (page.BindingContext is IHasOwnerPage hop)
                hop.OnOwnerPageDisappearing();
        }

        //public TPage MakePage<TPage, TViewModel>(Action<TViewModel> setState) where TPage : Page
        //{
        //    TPage page = (TPage)_typeFactory.Invoke(typeof(TPage));
        //    TViewModel vm = (TViewModel)_typeFactory.Invoke(typeof(TViewModel));
        //    setState?.Invoke(vm);
        //    page.BindingContext = vm;

        //    return page;
        //}
        public (TPage page, TViewModel viewModel) GetMvvmPage<TPage, TViewModel>(GetMvvmPageMode mode)
            where TPage : Page
            where TViewModel : class
        {
            TPage page = (TPage)_typeFactory(typeof(TPage));
            TViewModel vm = null;

            GetMvvmPageMode actualMode = mode;
            if (actualMode == GetMvvmPageMode.Default)
                actualMode = GetMvvmPageMode.Singleton;

            switch (actualMode)
            {
                case GetMvvmPageMode.Default:
                    {
                        throw new InvalidOperationException("This cannot happen");
                    }
                case GetMvvmPageMode.Singleton:
                    {
                        vm = (TViewModel)_typeFactory(typeof(TViewModel));
                        if (page.BindingContext == null)
                            page.BindingContext = vm;

                        else if (page.BindingContext != vm)
                            throw new InvalidOperationException("Page already has a different viewModel instance");

                        page.BindingContext = vm;
                    }
                    break;
                case GetMvvmPageMode.Manual:
                    {
                        vm = (TViewModel)_typeFactory(typeof(TViewModel));
                        if (page.BindingContext != vm)
                            if (page.BindingContext != null)
                                throw new InvalidOperationException("Page already has a BindingContext");

                        page.BindingContext = vm;
                    }
                    break;
                case GetMvvmPageMode.Auto:
                    {
                        if (page.BindingContext == null)
                        {
                            vm = (TViewModel)_typeFactory(typeof(TViewModel));
                            page.BindingContext = vm;
                        }
                        else if ((page.BindingContext is TViewModel) == false)
                        {
                            throw new InvalidOperationException("Page has an unsuitable BindingContext");
                        }
                        else
                        {
                            vm = (TViewModel)page.BindingContext;
                        }
                    }
                    break;
                case GetMvvmPageMode.Page:
                    {
                        if (page.BindingContext == null)
                        {
                            throw new InvalidOperationException("Page does not have a BindingContext");
                        }
                        if (page.BindingContext is TViewModel pvm)
                        {
                            vm = pvm;
                        }
                        else
                        {
                            throw new InvalidOperationException("Page has an unsuitable BindingContext");
                        }
                    }
                    break;
                case GetMvvmPageMode.Force:
                    {
                        vm = (TViewModel)_typeFactory(typeof(TViewModel));
                        page.BindingContext = vm;
                    }
                    break;
            }
            return (page, vm);
        }

        public TPage GetPage<TPage>() where TPage : Page
        {
            TPage page = (TPage)_typeFactory.Invoke(typeof(TPage));
            return page;
        }
        public TViewModel GetViewModel<TViewModel>() where TViewModel : class
        {
            TViewModel vm = (TViewModel)_typeFactory.Invoke(typeof(TViewModel));
            return vm;
        }

        public async Task<TViewModel> PushPageAsync<TPage, TViewModel>(Func<TViewModel, Task> initViewModelActionAsync, bool isModal, GetMvvmPageMode mode)
    where TPage : Page
    where TViewModel : class
        {
            var mvvmPage = GetMvvmPage<TPage, TViewModel>(mode);

            if (initViewModelActionAsync != null)
            {
                await initViewModelActionAsync(mvvmPage.viewModel);
            }

            await PushPageAsync(mvvmPage.page, isModal);

            return mvvmPage.viewModel;
        }

        //public async Task<Page> PushPageAsync<TPage, TViewModel>(Action<TViewModel> setStateAction, bool isModal = false) where TPage : Page
        //{
        //    TPage newPage = MakePage<TPage, TViewModel>(setStateAction);
        //    return await PushPageAsync(newPage, isModal);
        //}
        public async Task<TViewModel> PushPageAsync<TPage, TViewModel>(Action<TViewModel> initViewModelAction, bool isModal, GetMvvmPageMode mode)
    where TPage : Page
    where TViewModel : class
        {
            var mvvmPage = GetMvvmPage<TPage, TViewModel>(mode);

            if (initViewModelAction != null)
            {
                initViewModelAction(mvvmPage.viewModel);
            }

            await PushPageAsync(mvvmPage.page, isModal);

            return mvvmPage.viewModel;
        }


        public async Task PushPageAsync(Page page, bool isModal)
        {
            var hop = page.BindingContext as IHasOwnerPage;
            if (isModal == false)
            {
                hop?.OnOwnerPagePushing(isModal);
                await CurrentNavigationPage.PushAsync(page, true);
                hop?.OnOwnerPagePushed(isModal);
            }
            else
            {
                hop?.OnOwnerPagePushing(isModal);
                await CurrentNavigationPage.PushModalAsync(page, true);
                hop?.OnOwnerPagePushed(isModal);
            }
        }

        //public async Task<Page> PushPageAsync<TPage>(Action<object> setStateAction, bool isModal = false) where TPage : Page
        //{
        //    var mvvmPage = GetMvvmPage<TPage, object>();
        //    return await PushPageAsync(newPage, isModal);
        //}



        public async Task PopAsync(bool isModal, bool animated = true)
        {
            if (!isModal)
            {
                var page = CurrentNavigationPage.NavigationStack.Last();
                var hop = page.BindingContext as IHasOwnerPage;
                hop?.OnOwnerPagePopping(isModal);
                await CurrentNavigationPage.PopAsync(animated);
                hop?.OnOwnerPagePopped(isModal);
            }
            else
            {
                var page = CurrentNavigationPage.ModalStack.Last();
                var hop = page.BindingContext as IHasOwnerPage;
                hop?.OnOwnerPagePopping(isModal);
                await CurrentNavigationPage.PopModalAsync(animated);
                hop?.OnOwnerPagePopped(isModal);
            }
        }
        public async Task PopToRootAsync(bool animated = false)
        {
            var navStack = CurrentNavigationPage.NavigationStack;

            // CurrentNavigationPage.PopToRootAsync does not raise OnPageDisappearing on the top page,
            // so we must do it here.
            // Odd, given the appearing / disappearing methods are called when backgrounding, foregrounding etc.
            if (navStack.Count > 1)
            {
                var topPage = navStack[navStack.Count - 1];
                (topPage.BindingContext as IHasOwnerPage)?.OnOwnerPageDisappearing();
            }
            await CurrentNavigationPage.PopToRootAsync(animated);
        }
    }
}
