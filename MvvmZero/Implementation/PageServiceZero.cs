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
        private Page _oldMainPage;
        Func<object, Page> _pageResolver;

        private INavigation CurrentNavigationPage => _navigationGetter();


        /// <summary>
        /// Creates a PageServiceZero associated with the provided NavigationPage.
        /// Uses a Func to get the INavigation for Push operations to allow
        /// multiple nav stacks when using a MasterDetail page or similar architecture.
        /// </summary>
        /// <param name="navigationGetter">Function that returns the current INavigation</param>
        /// <param name="typeFactory">Function that returns an instance of a given type.</param>
        /// <param name="pageResolver">If using PushViewModelAsync you must provide a Function that returns a Page for a given ViewModel type</param>
        public PageServiceZero(Func<INavigation> navigationGetter, Func<Type, object> typeFactory, Func<object, Page> pageResolver = null)
        {
            _navigationGetter = navigationGetter;
            _typeFactory = typeFactory;
            _pageResolver = pageResolver;
        }

        private void Init()
        {
            // NOTE: Overkill, because AFAIK Application.Current cannot change once set.
            // NOTE: => _currentApplication will only ever be null here.
            if (_currentApplication != null)
            {
                _currentApplication.PageAppearing -= Current_PageAppearing;
                _currentApplication.PageDisappearing -= Current_PageDisappearing;

                if (_currentApplication.MainPage != null)
                {
                    _currentApplication.MainPage.ChildAdded -= CurrentApplication_ChildAdded;
                    _currentApplication.MainPage.ChildRemoved -= CurrentApplication_ChildRemoved;
                }
                _currentApplication.PropertyChanged -= CurrentApplication_PropertyChanged;

                _currentApplication.ModalPushed -= CurrentApplication_ModalPushed;
                _currentApplication.ModalPopped -= CurrentApplication_ModalPopped;
            }

            var currentApplication = Application.Current;

            if (currentApplication != null)
            {
                currentApplication.PageAppearing += Current_PageAppearing;
                currentApplication.PageDisappearing += Current_PageDisappearing;

                // TODO: Can currentApplication.MainPage be null?
                // TODO: If so (I suspect it can), we should subscribe and unsubscribe when it changes.

                if (currentApplication.MainPage != null)
                {
                    currentApplication.MainPage.ChildAdded += CurrentApplication_ChildAdded;
                    currentApplication.MainPage.ChildRemoved += CurrentApplication_ChildRemoved;
                }

                currentApplication.PropertyChanged += CurrentApplication_PropertyChanged;

                currentApplication.ModalPushed += CurrentApplication_ModalPushed;
                currentApplication.ModalPopped += CurrentApplication_ModalPopped;
            }
            _currentApplication = currentApplication;
        }

        private void CurrentApplication_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine($"PSZ:INPC:{e.PropertyName}");

            if (e.PropertyName == nameof(_currentApplication.MainPage))
            {
                if (_oldMainPage != null)
                {
                    _oldMainPage.ChildAdded -= CurrentApplication_ChildAdded;
                    _oldMainPage.ChildRemoved -= CurrentApplication_ChildRemoved;
                    _oldMainPage = _currentApplication.MainPage;
                }
                if (_currentApplication.MainPage != null)
                {
                    _currentApplication.MainPage.ChildAdded += CurrentApplication_ChildAdded;
                    _currentApplication.MainPage.ChildRemoved += CurrentApplication_ChildRemoved;
                }
            }
        }

        private void CurrentApplication_ModalPopped(object sender, ModalPoppedEventArgs e)
        {
            Debug.WriteLine($"Modal Removed: {e.Modal}");

            if (e.Modal is Page page)
                if (page.BindingContext is IHasOwnerPage hop)
                    hop.OnOwnerPagePopped(true);
        }

        private void CurrentApplication_ModalPushed(object sender, ModalPushedEventArgs e)
        {
            Debug.WriteLine($"Modal Added: {e.Modal}");

            if (e.Modal is Page page)
                if (page.BindingContext is IHasOwnerPage hop)
                    hop.OnOwnerPagePushed(true);
        }

        private void CurrentApplication_ChildRemoved(object sender, ElementEventArgs e)
        {
            Debug.WriteLine($"Removed: {e.Element}");

            //((e.Element as Page)?.BindingContext as IHasOwnerPage)?.OnOwnerPagePopped(false);

            if (e.Element is Page page)
                if (page.BindingContext is IHasOwnerPage hop)
                    hop.OnOwnerPagePopped(false);
        }

        private void CurrentApplication_ChildAdded(object sender, ElementEventArgs e)
        {
            Debug.WriteLine($"Added: {e.Element}");

            if (e.Element is Page page)
                if (page.BindingContext is IHasOwnerPage hop)
                    hop.OnOwnerPagePushed(false);
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

        public (TPage page, TViewModel viewModel) GetMvvmPage<TPage, TViewModel>(Action<TViewModel> initViewModelAction)
            where TPage : Page
            where TViewModel : class
        {
            TPage page = GetPage<TPage>();
            TViewModel vm = GetViewModel<TViewModel>();

            initViewModelAction?.Invoke(vm);
            // Vm is initialised *before* setting the BindingContext, so any Bindings to non INPC properties or OneShot Bindings are correct.
            page.BindingContext = vm;

            return (page, vm);
        }

        public TPage GetPage<TPage>() where TPage : Page
        {
            if (_currentApplication != Application.Current)
                Init();

            TPage page = (TPage)_typeFactory(typeof(TPage));
            return page;
        }

        public TViewModel GetViewModel<TViewModel>() where TViewModel : class
        {
            TViewModel vm = (TViewModel)_typeFactory(typeof(TViewModel));
            return vm;
        }
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<TViewModel> PushPageAsync<TPage, TViewModel>(Action<TViewModel> initViewModelAction, bool isModal, bool animated)
    where TPage : Page
    where TViewModel : class
        {
            var mvvmPage = GetMvvmPage<TPage, TViewModel>(initViewModelAction);

            await PushPageAsync(mvvmPage.page, isModal, animated);
            return mvvmPage.viewModel;
        }

        public async Task PushPageAsync(Page page, bool isModal, bool animated)
        {
            if (page.BindingContext is IHasOwnerPage hop)
                hop.OnOwnerPagePushing(isModal);

            if (isModal == false)
            {
                await CurrentNavigationPage.PushAsync(page, animated);
            }
            else
            {
                await CurrentNavigationPage.PushModalAsync(page, animated);
            }
        }
        
        public async Task<TViewModel> PushViewModelAsync<TViewModel>(Action<TViewModel> initViewModelAction, bool isModal, bool animated) where TViewModel : class
        {
            if (_pageResolver == null)
                throw new InvalidOperationException("Please provide a PageResolver to the PageService constructor to use PushViewModelAsync");

            TViewModel vm = GetViewModel<TViewModel>();
            var page = _pageResolver(vm);
            initViewModelAction?.Invoke(vm);
            page.BindingContext = vm;
            await PushPageAsync(page, isModal, animated);
            return vm;
        }
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Don't do anything fancy in PopAsync because the system can bypass this method and pop stuff directly.
        public async Task PopAsync(bool isModal, bool animated = true)
        {
            if (!isModal)
                await CurrentNavigationPage.PopAsync(animated);
            else
                await CurrentNavigationPage.PopModalAsync(animated);
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

        public void RemovePageBelowTop()
        {
            if (CurrentNavigationPage != null)
            {
                int index = CurrentNavigationPage.NavigationStack.Count - 2;
                if (index >= 0)
                {
                    var page = CurrentNavigationPage.NavigationStack[index];
                    CurrentNavigationPage.RemovePage(page);
                }
            }
        }
    }
}
