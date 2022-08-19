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

        /// <summary>
        /// Makes a TPage with a BindingContext set to a TViewModel
        /// </summary>
        /// <typeparam name="TPage">The type of page to return</typeparam>
        /// <typeparam name="TViewModel">The type of ViewModel found on the page BindingContext</typeparam>
        /// <param name="mode">The strategy used when acquiring the ViewModel</param>
        /// <returns>A Tuple containing a reference to the TPage and a reference to the TViewModel found on the page BindingContext</returns>
        (TPage page, TViewModel viewModel) GetMvvmPage<TPage, TViewModel>(Action<TViewModel> initViewModelAction)
            where TPage : Page
            where TViewModel : class;

        /// <summary>
        /// Returns a Page of type TPage
        /// </summary>
        /// <typeparam name="TPage"></typeparam>
        /// <returns></returns>
        TPage GetPage<TPage>() where TPage : Page;

        /// <summary>
        /// Returns a ViewModel of type TViewModel
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <returns></returns>
        TViewModel GetViewModel<TViewModel>() where TViewModel : class;

        /// <summary>
        /// Pushes a TPage onto the navigation stack whose BindingContext is set to a TViewModel
        /// </summary>
        /// <typeparam name="TPage"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="initViewModelActionAsync">An async Action used to initialise the ViewModel before it it's page is presented</param>
        /// <param name="isModal">Whether the Page should be presented on the modal stack</param>
        /// <param name="animated">Whether the page should appear using the system animation</param>
        /// <returns>The ViewModel found on the Page's BindingContext</returns>
        //Task<TViewModel> PushPageAsync<TPage, TViewModel>(Func<TViewModel, Task> initViewModelActionAsync, bool isModal = false, bool animated = true)
        //    where TPage : Page
        //    where TViewModel : class;

        /// <summary>
        /// Pushes a TPage onto the navigation stack whose BindingContext is set to a TViewModel
        /// </summary>
        /// <typeparam name="TPage"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="initViewModelActionAsync">An Action used to initialise the ViewModel before it it's page is presented</param>
        /// <param name="isModal">Whether the Page should be presented on the modal stack</param>
        /// <param name="animated">Whether the page should appear using the system animation</param>
        /// <returns>The ViewModel found on the Page's BindingContext</returns>
        Task<TViewModel> PushPageAsync<TPage, TViewModel>(Action<TViewModel> initViewModelAction, bool isModal = false, bool animated = true)
            where TPage : Page
            where TViewModel : class;

        /// <summary>
        /// Pushes a Page onto the navigation stack
        /// </summary>
        /// <param name="page">The Page to push</param>
        /// <param name="isModal">Whether the Page should be presented on the modal stack</param>
        /// <param name="animated">Whether the page should appear using the system animation</param>
        /// <returns>The ViewModel found on the Page's BindingContext</returns>
        Task PushPageAsync(Page page, bool isModal, bool animated = true);
        //Task PushPageAsync<TPage>(Action<object> setStateAction, bool isModal = false) where TPage : Page;

        /// <summary>
        /// NOTE: To use this method, the PageService needs to know how to get a Page for a given ViewModel.
        /// PageServiceZero can do this if you provide a page-resolver to the constructor.
        /// This method finds a suitable Page for the TViewModel, sets the page BindingContext to a TViewModel, 
        /// then pushes the page onto the navigation stack.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="initViewModelActionAsync">An Action used to initialise the ViewModel before it it's page is presented</param>
        /// <param name="isModal">Whether the Page should be presented on the modal stack</param>
        /// <param name="animated">Whether the page should appear using the system animation</param>
        /// <returns>The ViewModel found on the Page's BindingContext</returns>
        Task<TViewModel> PushViewModelAsync<TViewModel>(Action<TViewModel> initViewModelAction, bool isModal, bool animated) where TViewModel : class;


        /// <summary>
        /// Pops the top page from the navigation stack
        /// </summary>
        /// <param name="isModal">Whether the Page should be presented on the modal stack</param>
        /// <param name="animated">Whether the page should appear using the system animation</param>
        /// <returns>A Task that can be awaited</returns>
        Task PopAsync(bool isModal, bool animated = true);


        /// <summary>
        /// Pops the all pages from the navigation stack
        /// </summary>
        /// <param name="animated">Whether the page should appear using the system animation</param>
        /// <returns>A Task that can be awaited</returns>
        Task PopToRootAsync(bool animated = true);

        void RemovePageBelowTop();

        //Task<TViewModel> PushViewModelAsync<TViewModel>(Func<TViewModel, Task> initViewModelAction, bool isModal, bool animated) where TViewModel : class;
        //void RemovePageAtIndex(int index);
        //void GetNavigationStackCount(bool isModal = false);
    }
}
