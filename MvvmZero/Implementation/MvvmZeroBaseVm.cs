using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using FunctionZero.CommandZero;

namespace FunctionZero.MvvmZero
{
    /// <summary>
    /// If you get a UWP xaml compiler error after deriving from this class,
    /// reference this library directly in your UWP project to resolve it.
    /// </summary>
    public abstract class MvvmZeroBaseVm : IGuard, INotifyPropertyChanged, IHasOwnerPage
    {
        private readonly IGuard _guardImplementation;
        public event PropertyChangedEventHandler PropertyChanged;

        public MvvmZeroBaseVm()
        {
            _guardImplementation = new BasicGuard();
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

        public virtual void OwnerPageAppearing()
        {
            Debug.WriteLine($"{GetType()} Appearing");
        }

        public virtual void OwnerPageDisappearing()
        {
            Debug.WriteLine($"{GetType()} Disappearing");
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
