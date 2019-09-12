/*
MIT License

Copyright(c) 2016 - 2019 Function Zero Ltd

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
SOFTWARE.using System;
*/

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
                    _pageCreateAction?.Invoke(page);

                    if (parameter is IHasOwnerPage<TEnum> tHasOwnerPage)
                    {
                        page.Appearing += (s, e) => tHasOwnerPage.OwnerPageAppearing(pageKey, this.CurrentNavigationPage?.StackDepth);
                        page.Disappearing += (s, e)=> tHasOwnerPage.OwnerPageDisappearing();
                    }
                    return page;
                }
                throw new InvalidOperationException($"Page {pageKey} does not exist.");
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
        }
    }
}
