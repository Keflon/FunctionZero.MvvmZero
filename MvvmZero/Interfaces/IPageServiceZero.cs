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
        GetMvvmPageMode DefaultMvvmPageMode { get; set; }
        /// <summary>
        /// Call this only if you create a PageService before Application.Current is set, 
        /// and you don't have the application instance to inject to the constructor at the time of creation.
        /// </summary>
        /// <param name="currentApplication"></param>
        void Init(Application currentApplication);


        /// <summary>
        /// Makes a TPage with a BindingContext set to a TViewModel
        /// </summary>
        /// <typeparam name="TPage">The type of page to return</typeparam>
        /// <typeparam name="TViewModel">The type of ViewModel found on the page BindingContext</typeparam>
        /// <param name="mode">The strategy used when acquiring the ViewModel</param>
        /// <returns>A Tuple containing a reference to the TPage and a reference to the TViewModel found on the page BindingContext</returns>
        (TPage page, TViewModel viewModel) GetMvvmPage<TPage, TViewModel>(GetMvvmPageMode mode = GetMvvmPageMode.Default)
            where TPage : Page
            where TViewModel : class;


        TPage GetPage<TPage>() where TPage : Page;

        TViewModel GetViewModel<TViewModel>() where TViewModel : class;

        Task<TViewModel> PushPageAsync<TPage, TViewModel>(Func<TViewModel, Task> initViewModelActionAsync, bool isModal = false, GetMvvmPageMode mode = GetMvvmPageMode.Default)
            where TPage : Page
            where TViewModel : class;

        Task<TViewModel> PushPageAsync<TPage, TViewModel>(Action<TViewModel> initViewModelAction, bool isModal = false, GetMvvmPageMode mode = GetMvvmPageMode.Default)
            where TPage : Page
            where TViewModel : class;

        Task PushPageAsync(Page page, bool isModal);
        //Task PushPageAsync<TPage>(Action<object> setStateAction, bool isModal = false) where TPage : Page;
        Task PopAsync(bool isModal, bool animated = true);
        Task PopToRootAsync(bool animated = true);
#endif
    }

    public enum GetMvvmPageMode
    {
        /// <summary>
        /// Use DefaultMvvmPageMode
        /// </summary>
        Default = 0,
        /// <summary>
        /// Gets a reference to a TViewModel
        /// If the Page has a null BindingContext, sets it to the reference
        /// Throws an exception if the Page already has a BindingContext that is different to the TViewModel reference
        /// </summary>
        Singleton,
        /// <summary>
        /// Assumes the Page has a null BindingContext and assigns a TViewModel
        /// Throws an exception if the Page already has a BindingContext
        /// </summary>
        Manual,
        /// <summary>
        /// If the Page does not have a BindingContext already, sets the BindingContext to a TViewModel
        /// Throws an exception if the Page already has a BindingContext that is-not-a TViewModel
        /// </summary>
        Auto,
        /// <summary>
        /// Assumes the Page will already have a BindingContext that is-a TViewModel
        /// Throws an exception if this is not the case
        /// </summary>
        Page,

        /// <summary>
        /// Overwrites page.BindingContext with a TViewModel irrespective of what page.BindingContext may initially refer to
        /// </summary>
        Force
    }
}
