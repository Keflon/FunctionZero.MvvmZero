﻿/*
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
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FunctionZero.MvvmZero.Interfaces
{
    //public interface IPageServiceZero<TEnum> where TEnum  : Enum
    //{
    //    void SetPage(Page page);
    //    Page SetPage(TEnum pageKey, object parameter);
    //    Page CurrentPage { get; }
    //    Task<Page> PushPageAsync(TEnum pageKey, object parameter, bool killExistingNavigationPage = false);
    //    Task<Page> PushModalPageAsync(TEnum pageKey, object parameter);
    //    void Register(TEnum pageKey, Func<object, Page> pageMaker);
    //    Task PopAsync(bool animated = true);
    //    Task PopModalAsync(bool animated = true);
    //    Task PopToDepthAsync(int desiredDepth, bool animated = true);

    //}

    //public interface IFlowPageServiceZero
    //{
    //    void SetPage(Page page);
    //    Page SetPage<TPage, TViewModel>(Action<TViewModel> setState) where TPage : Page;
    //    Page CurrentPage { get; }
    //    TPage MakePage<TPage, TViewModel>(Action<TViewModel> setState) where TPage : Page;
    //    Task<Page> PushPageAsync<TPage, TViewModel>(Action<TViewModel> setState, bool killExistingNavigationPage = false) where TPage : Page;
    //    void RegisterTypeFactory(Func<Type, object> typeFactory);
    //    Task<Page> PushModalPageAsync<TPage, TViewModel>(Action<TViewModel> setState);
    //    //void Register(TEnum pageKey, Func<object, Page> pageMaker);
    //    Task PopAsync(bool animated = true);
    //    Task PopModalAsync(bool animated = true);
    //    Task PopToDepthAsync(int desiredDepth, bool animated = true);

    //}

    public interface IFlowPageServiceZero
    {
        void SetPage(Page page);
        Page CurrentPage { get; }
        void RegisterTypeFactory(Func<Type, object> typeFactory);
        TPage MakePage<TPage, TViewModel>(Action<TViewModel> setState) where TPage : Page;
        TPage SetPage<TPage, TViewModel>(Action<TViewModel> setStateAction) where TPage : Page;
        Task<Page> PushPageAsync(Page page, bool isModal);
        Task<Page> PushPageAsync<TPage, TViewModel>(Action<TViewModel> setStateAction, bool isModal = false) where TPage : Page;
        Task PopAsync(bool isModal, bool animated = true);
    }

    //public interface IFlowPageZero<TInput, TOutput>
    //{
    //    void SetState(TInput state);
    //    TInput GetState();
    //    TOutput GetResult();
    //}


}
