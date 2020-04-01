/*
MIT License

Copyright(c) 2016 - 2020 Function Zero Ltd

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
using FunctionZero.MvvmZero.Interfaces;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FunctionZero.MvvmZero
{
    public class PageServiceZero : IPageServiceZero
    {
        public Page CurrentPage => _application.MainPage;

        private readonly Application _application;
        private readonly Action<Page> _pageCreateAction;
        private Func<Type, object> _typeFactory;

        public NavigationPage CurrentNavigationPage => CurrentPage as NavigationPage;

        public PageServiceZero(Application application, Func<Type, object> typeFactory, Action<Page> pageCreateAction)
        {
            _application = application;
            _pageCreateAction = pageCreateAction;
            _typeFactory = typeFactory;
        }

        public TPage MakePage<TPage, TViewModel>(Action<TViewModel> setState) where TPage : Page
        {
            TPage page = (TPage)_typeFactory.Invoke(typeof(TPage));
            TViewModel vm = (TViewModel)_typeFactory.Invoke(typeof(TViewModel));
            setState?.Invoke(vm);
            page.BindingContext = vm;

            _pageCreateAction?.Invoke(page);

            if (vm is IHasOwnerPage tHasOwnerPage)
            {
                page.Appearing += (s, e) => tHasOwnerPage.OwnerPageAppearing(this.CurrentNavigationPage?.StackDepth);
                page.Disappearing += (s, e) => tHasOwnerPage.OwnerPageDisappearing();
            }
            return page;
        }

        public async Task PopAsync(bool isModal, bool animated = true)
        {
            if (isModal)
                await CurrentNavigationPage.PopAsync(animated);
            else
                await CurrentNavigationPage.Navigation.PopModalAsync(animated);
        }

        public async Task<Page> PushPageAsync(Page page, bool isModal)
        {
            if (isModal == false)
            {
                if (CurrentNavigationPage == null /* || killExistingNavigationPage */)
                {
                    var navPage = new NavigationPage(page);
                    // When a Page is popped, tell it to release any bindings to the view model.
                    navPage.Popped += (s, e) => e.Page.BindingContext = null;
                    this.SetPage(navPage);
                }
                else
                {
                    await CurrentNavigationPage.Navigation.PushAsync(page, true);
                }
            }
            else
            {
                if (CurrentNavigationPage == null)
                    this.SetPage(new NavigationPage());

                await CurrentNavigationPage.Navigation.PushModalAsync(page, true);
            }

            return page;
        }

        public async Task<Page> PushPageAsync<TPage, TViewModel>(Action<TViewModel> setStateAction, bool isModal = false) where TPage : Page
        {
            TPage newPage = MakePage<TPage, TViewModel>(setStateAction);
            return await PushPageAsync(newPage, isModal);
        }

        public void SetPage(Page page)
        {
            _application.MainPage = page;
        }

        public TPage SetPage<TPage, TViewModel>(Action<TViewModel> setStateAction) where TPage : Page
        {
            TPage newPage = MakePage<TPage, TViewModel>(setStateAction);
            SetPage(newPage);
            return newPage;
        }
    }
}
