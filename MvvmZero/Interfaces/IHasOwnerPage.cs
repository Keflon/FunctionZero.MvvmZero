﻿/*
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
using System.ComponentModel;

namespace FunctionZero.MvvmZero
{
    /// <summary>
    /// If the ViewModel of a Page supports this interface, these methods are called by the page service.
    /// </summary>
    public interface IHasOwnerPage : INotifyPropertyChanged
    {
        /// <summary>
        /// Tells us whether the owner page is visible.
        /// Implementors must raise INPC.
        /// </summary>
        bool IsOwnerPageVisible { get; }
        /// <summary>
        /// Tells us whether the owner page is on a navigation stack.
        /// Implementors must raise INPC.
        /// </summary>
        bool IsOnNavigationStack { get; }
        /// <summary>
        /// Lifecycle for a Page
        /// </summary>
        void OnOwnerPageAppearing();
        void OnOwnerPageDisappearing();
        void OnOwnerPagePushing(bool isModal);
        //void OnOwnerPagePopping(bool isModal);
        void OnOwnerPagePushed(bool isModal);
        void OnOwnerPagePopped(bool isModal);
    }
}
