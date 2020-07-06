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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FunctionZero.MvvmZero
{
    public class PageServiceZero : IPageServiceZero
    {
        private readonly Action<Page> _pageCreateAction;
        private readonly Func<INavigation> _navigationGetter;
        private Func<Type, object> _typeFactory;

        private INavigation CurrentNavigationPage => _navigationGetter();

        /// <summary>
        /// Creates a PageServiceZero associated with the provided NavigationPage.
        /// Uses a Func to get the INavigation for Push operations to allow
        /// multiple nav stacks when using a MasterDetail page or similar architecture.
        /// </summary>
        /// <param name="navPage"></param>
        /// <param name="typeFactory"></param>
        /// <param name="pageCreateAction"></param>
        public PageServiceZero(Func<INavigation> navigationGetter, Func<Type, object> typeFactory, Action<Page> pageCreateAction)
        {
            _navigationGetter = navigationGetter;
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
            Page page;

            if (!isModal)
                page = await CurrentNavigationPage.PopAsync(animated);
            else
                page = await CurrentNavigationPage.PopModalAsync(animated);

            DetachFromPage(page);
        }

        public async Task<Page> PushPageAsync(Page page, bool isModal)
        {
            AttachToPage(page);

            if (isModal == false)
                await CurrentNavigationPage.PushAsync(page, true);
            else
                await CurrentNavigationPage.PushModalAsync(page, true);

            return page;
        }

        private void Page_Disappearing(object sender, EventArgs e)
        {
            var page = (Page)sender;
            Debug.WriteLine($"XXX {page} disappearing.");
            if (page.BindingContext is IHasOwnerPage hop)
                hop.OwnerPageDisappearing();
        }

        private void Page_Appearing(object sender, EventArgs e)
        {
            var page = (Page)sender;
            Debug.WriteLine($"XXX {page} appearing.");

            if (page.BindingContext is IHasOwnerPage hop)
                hop.OwnerPageAppearing();
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

        public async Task PopToRootAsync(bool animated = false)
        {
            var pages = new List<Page>(CurrentNavigationPage.NavigationStack);

            await CurrentNavigationPage.PopToRootAsync(animated);

            // Detach from the pages we've just discarded.
            // Do not detach from the top page because it is not popped.
            for (int c = pages.Count - 1; c > 0; c--)
                DetachFromPage(pages[c]);
        }

        private void DetachFromPage(Page thePage)
        {
            Debug.WriteLine($"Letting go of {thePage}");
            thePage.Appearing -= Page_Appearing;
            thePage.Disappearing -= Page_Disappearing;
        }

        private void AttachToPage(Page page)
        {
            page.Appearing += Page_Appearing;
            page.Disappearing += Page_Disappearing;
        }
    }
}
