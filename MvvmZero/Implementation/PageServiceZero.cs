using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FunctionZero.MvvmZero.Interfaces;
using Xamarin.Forms;

namespace FunctionZero.MvvmZero.Implementation
{
    namespace FunctionZero.PageServiceZero
    {
        public class PageServiceZero<TEnum> : IPageServiceZero<TEnum> where TEnum : Enum
        {
            private readonly Dictionary<TEnum, Func<object, Page>> _pagesByKey =
                   new Dictionary<TEnum, Func<object, Page>>();

            private readonly Application _application;
            private readonly Action<Page> _pageCreateAction;

            public Page CurrentPage => _application.MainPage;
            public NavigationPage CurrentNavigationPage => CurrentPage as NavigationPage;

            public PageServiceZero(Application application, Action<Page> pageCreateAction)
            {
                _application = application;
                _pageCreateAction = pageCreateAction;
            }

            public void SetPage(Page page)
            {
                _application.MainPage = page;
            }

            private Page MakePage(TEnum pageKey, object parameter)
            {
                if (_pagesByKey.TryGetValue(pageKey, out var pageMaker))
                {
                    Page page = pageMaker.Invoke(parameter);

                    //if(parameter is IHasOwnerPage iop)
                    //    iop.OwnerPageKey = Convert.ToInt32(pageKey);

                    if (parameter is IHasOwnerPage<TEnum> hop)
                        hop.OwnerPageKey = pageKey;
                    _pageCreateAction?.Invoke(page);

                    page.Appearing += NewPageOnAppearing;
                    page.Disappearing += NewPageOnDisappearing;

                    return page;
                }
                throw new Exception($"Page {pageKey} does not exist.");
            }

            public Page SetPage(TEnum pageKey, object parameter)
            {
                Page newPage = MakePage(pageKey, parameter);
                _application.MainPage = newPage;
                return newPage;
            }

            public async Task<Page> PushPageAsync(TEnum pageKey, object parameter, bool killExistingNavigationPage = false)
            {
                Page newPage = MakePage(pageKey, parameter);
                if (CurrentNavigationPage == null || killExistingNavigationPage)
                {
                    var navPage = new NavigationPage(newPage);
                    this.SetPage(navPage);
                }
                else
                {
                    await CurrentNavigationPage.Navigation.PushAsync(newPage, true);
                }

                if (parameter is IHasOwnerPage<TEnum> hop)
                    hop.PageDepth = this.CurrentNavigationPage?.StackDepth;

                return newPage;
            }

            public async Task<Page> PushModalPageAsync(TEnum pageKey, object parameter)
            {
                Page newPage = MakePage(pageKey, parameter);
                if (CurrentNavigationPage == null)
                    this.SetPage(new NavigationPage());

                await CurrentNavigationPage.Navigation.PushModalAsync(newPage, true);

                return newPage;
            }

            public void Register(TEnum pageKey, Func<object, Page> pageMaker)
            {
                _pagesByKey.Add(pageKey, pageMaker);
            }

            public async Task PopAsync(bool animated = true)
            {
                CurrentNavigationPage.CurrentPage.BindingContext = null;
                await CurrentNavigationPage.Navigation.PopAsync(animated);
            }

            public async Task PopToDepthAsync(int desiredDepth, bool animated = true)
            {
                while (CurrentNavigationPage.StackDepth > desiredDepth + 1)
                {
                    CurrentNavigationPage.CurrentPage.BindingContext = null;
                    CurrentNavigationPage.Navigation.RemovePage(CurrentNavigationPage.CurrentPage);
                }
                await PopAsync(animated);
            }

            public async Task PopModalAsync(bool animated = true)
            {
                await CurrentNavigationPage.Navigation.PopModalAsync(true);
            }

            private void NewPageOnDisappearing(object sender, EventArgs e)
            {
                Page newPage = (Page)sender;
                //if (newPage.BindingContext is IHasOwnerPage hasOwnerPage)
                //    hasOwnerPage.OwnerPageDisappearing();
                if (newPage.BindingContext is IHasOwnerPage<TEnum> tHasOwnerPage)
                    tHasOwnerPage.OwnerPageDisappearing();
            }

            private void NewPageOnAppearing(object sender, EventArgs e)
            {
                Page newPage = (Page)sender;
                //if (newPage.BindingContext is IHasOwnerPage hasOwnerPage)
                //    hasOwnerPage.OwnerPageAppearing();
                if (newPage.BindingContext is IHasOwnerPage<TEnum> tHasOwnerPage)
                    tHasOwnerPage.OwnerPageAppearing();
            }
        }
    }
}
