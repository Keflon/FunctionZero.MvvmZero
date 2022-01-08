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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using FunctionZero.CommandZero;
using FunctionZero.MvvmZero.Services;

namespace FunctionZero.MvvmZero
{
    /// <summary>
    /// If you get a UWP xaml compiler error after deriving from this class,
    /// reference this library directly in your UWP project to resolve it.
    /// </summary>
    public abstract class MvvmZeroBaseVm : IGuard, INotifyPropertyChanged, IHasOwnerPage
    {
        private readonly IGuard _guardImplementation;
        private bool _IsownerPageVisible;
        private bool _isOnNavigationStack;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler OwnerPageAppearing;
        public event EventHandler OwnerPageDisappearing;

        private readonly List<AutoPageTimer> _pageTimers;

        public MvvmZeroBaseVm()
        {
            _pageTimers = new List<AutoPageTimer>();
            _guardImplementation = new BasicGuard();
        }

        protected void AddPageTimer(int millisecondInterval, Action<object> callback, Action<Exception> exceptionHandler, object state)
        {
            _pageTimers.Add(new AutoPageTimer(this, millisecondInterval, callback, exceptionHandler, state));
        }

        public bool IsOwnerPageVisible
        {
            get => _IsownerPageVisible;
            private set => SetProperty(ref _IsownerPageVisible, value);
        }
        public bool IsOnNavigationStack
        {
            get => _isOnNavigationStack;
            private set => SetProperty(ref _isOnNavigationStack, value);
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnOwnerPageAppearing()
        {
            IsOwnerPageVisible = true;
            OwnerPageAppearing?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnOwnerPageDisappearing()
        {
            IsOwnerPageVisible = false;
            OwnerPageDisappearing?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnOwnerPagePushing(bool isModal)
        {
        }

        public virtual void OnOwnerPagePopping(bool isModal)
        {
        }

        public virtual void OnOwnerPagePushed(bool isModal)
        {
            IsOnNavigationStack = true;
        }

        public virtual void OnOwnerPagePopped(bool isModal)
        {
            IsOnNavigationStack = false;
        }

        public event EventHandler<GuardChangedEventArgs> GuardChanged
        {
            add => _guardImplementation.GuardChanged += value;
            remove => _guardImplementation.GuardChanged -= value;
        }

        public bool IsGuardRaised
        {
            get => _guardImplementation.IsGuardRaised;
            set => _guardImplementation.IsGuardRaised = value;
        }
    }
}
