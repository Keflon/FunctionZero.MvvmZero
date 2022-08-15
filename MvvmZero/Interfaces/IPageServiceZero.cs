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
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FunctionZero.MvvmZero
{
    public interface IPageServiceZero
    {
#if old
        /// <summary>
        /// Call this only if you create a PageService before Application.Current is set, 
        /// and you don't have the application instance to inject to the constructor at the time of creation.
        /// </summary>
        /// <param name="currentApplication"></param>
        void Init(Application currentApplication);
        TPage MakePage<TPage, TViewModel>(Action<TViewModel> setState) where TPage : Page;
        TPage MakePage<TPage>(Action<object> setState) where TPage : Page;

        TViewModel MakeViewModel<TViewModel>() where TViewModel : class;

        Task<Page> PushPageAsync(Page page, bool isModal);
        Task<Page> PushPageAsync<TPage, TViewModel>(Action<TViewModel> setStateAction, bool isModal = false) where TPage : Page;
        Task<Page> PushPageAsync<TPage>(Action<object> setStateAction, bool isModal = false) where TPage : Page;
        Task PopAsync(bool isModal, bool animated = true);
        Task PopToRootAsync(bool animated = true);

#else
        /// <summary>
        /// Makes a TPage with a BindingContext set to a TViewModel
        /// </summary>
        /// <typeparam name="TPage">The type of page to return</typeparam>
        /// <typeparam name="TViewModel">The type of ViewModel found on the page BindingContext</typeparam>
        /// <param name="mode">The strategy used when acquiring the ViewModel</param>
        /// <returns>A Tuple containing a reference to the TPage and a reference to the TViewModel found on the page BindingContext</returns>
        (TPage page, TViewModel viewModel) GetMvvmPage<TPage, TViewModel>()
            where TPage : Page
            where TViewModel : class;

        TPage GetPage<TPage>() where TPage : Page;

        TViewModel GetViewModel<TViewModel>() where TViewModel : class;

        Task<TViewModel> PushPageAsync<TPage, TViewModel>(Func<TViewModel, Task> initViewModelActionAsync, bool isModal = false, bool animated = true)
            where TPage : Page
            where TViewModel : class;

        Task<TViewModel> PushPageAsync<TPage, TViewModel>(Action<TViewModel> initViewModelAction, bool isModal = false, bool animated = true)
            where TPage : Page
            where TViewModel : class;

        Task PushPageAsync(Page page, bool isModal, bool animated = true);
        //Task PushPageAsync<TPage>(Action<object> setStateAction, bool isModal = false) where TPage : Page;
        Task PopAsync(bool isModal, bool animated = true);
        Task PopToRootAsync(bool animated = true);

        void RemovePageBelowTop();
        //void RemovePageAtIndex(int index);
        //void GetNavigationStackCount(bool isModal = false);
#endif
    }
}
