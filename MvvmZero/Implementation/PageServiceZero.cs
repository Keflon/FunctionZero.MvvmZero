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
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FunctionZero.MvvmZero
{
    public class PageServiceZero : IPageServiceZero
    {
        private readonly Application _application;
        private readonly Action<Page> _pageCreateAction;
        private Func<Type, object> _typeFactory;

        public Page CurrentPage => _application.MainPage;
        public NavigationPage CurrentNavigationPage => CurrentPage as NavigationPage;

        public PageServiceZero(Application application, Func<Type, object> typeFactory, Action<Page> pageCreateAction)
        {
               _application = application;
            _typeFactory = typeFactory;
            _pageCreateAction = pageCreateAction;
        }

        public TPage MakePage<TPage, TViewModel>(Action<TViewModel> setState) where TPage : Page
        {
            TPage page = (TPage)_typeFactory.Invoke(typeof(TPage));
            TViewModel vm = (TViewModel)_typeFactory.Invoke(typeof(TViewModel));
            setState?.Invoke(vm);
            page.BindingContext = vm;

            _pageCreateAction?.Invoke(page);

            return page;
        }
        public TPage MakePage<TPage>(Action<object> setState) where TPage : Page
        {
            TPage page = (TPage)_typeFactory.Invoke(typeof(TPage));
            setState?.Invoke(page.BindingContext);

            _pageCreateAction?.Invoke(page);

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
            if (CurrentNavigationPage == null)
                this.SetPage(CreateNavigationPage());

            if (isModal == false)
                await CurrentNavigationPage.Navigation.PushAsync(page, true);
            else
                await CurrentNavigationPage.Navigation.PushModalAsync(page, true);

            return page;
        }

        private NavigationPage CreateNavigationPage()
        {
            var navPage = new NavigationPage();
            navPage.ChildAdded += NavPage_ChildAdded;
            navPage.ChildRemoved += NavPage_ChildRemoved;
            return navPage;
        }

        private void NavPage_ChildRemoved(object sender, ElementEventArgs e)
        {
            Debug.WriteLine("Removed "+e.Element);

            Page page = (Page)e.Element;

            page.Appearing -= PageServiceZero_Appearing;
            page.Disappearing -= PageServiceZero_Disappearing;


            // When a Page is removed, tell it to release any bindings to the view model.
            page.BindingContext = null;
        }


        private void NavPage_ChildAdded(object sender, ElementEventArgs e)
        {
            Debug.WriteLine("Added " + e.Element);

            Page page = (Page)e.Element;

            page.Appearing += PageServiceZero_Appearing;
            page.Disappearing += PageServiceZero_Disappearing;
        }

        private void PageServiceZero_Disappearing(object sender, EventArgs e)
        {
            Page page = (Page)sender;

            if (page.BindingContext is IHasOwnerPage hop)
                hop.OwnerPageDisappearing();
        }

        private void PageServiceZero_Appearing(object sender, EventArgs e)
        {
            if (((Page)sender).BindingContext is IHasOwnerPage hop)
                hop.OwnerPageAppearing(this.CurrentNavigationPage?.StackDepth);
        }

        public async Task<Page> PushPageAsync<TPage, TViewModel>(Action<TViewModel> setStateAction, bool isModal = false) where TPage : Page
        {
            TPage newPage = MakePage<TPage, TViewModel>(setStateAction);
            return await PushPageAsync(newPage, isModal);
        }

        public async Task<Page> PushPageAsync<TPage>(Action<object> setStateAction, bool isModal = false) where TPage : Page
        {
            TPage newPage = MakePage<TPage>(setStateAction);
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

        public TPage SetPage<TPage>(Action<object> setStateAction) where TPage : Page
        {
            TPage newPage = MakePage<TPage>(setStateAction);
            SetPage(newPage);
            return newPage;
        }

        public async Task PopToRootAsync(bool animated = false)
        {
            await CurrentNavigationPage.PopToRootAsync(animated);
        }
    }
}
