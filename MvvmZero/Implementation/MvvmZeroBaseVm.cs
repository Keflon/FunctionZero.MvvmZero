using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using FunctionZero.MvvmZero.Commanding;
using FunctionZero.MvvmZero.Interfaces;


namespace FunctionZero.MvvmZero.Implementation
{
    /// <summary>
    /// If you get a UWP xaml compiler error after deriving from this class,
    /// reference this library directly in your UWP project to resolve it.
    /// </summary>
    public abstract class MvvmZeroBaseVm : IGuard, INotifyPropertyChanged, IHasOwnerPage
    {
        private readonly IGuard _guardImplementation;
        private int? _ownerPageKey;
        public event PropertyChangedEventHandler PropertyChanged;

        public MvvmZeroBaseVm()
        {
            _guardImplementation = new BasicGuard();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OwnerPageAppearing(int? pageKey, int? pageDepth)
        {
            OwnerPageKey = pageKey;
            Debug.WriteLine($"{GetType()} Appearing");
        }

        public virtual void OwnerPageDisappearing()
        {
            OwnerPageKey = -1;

            Debug.WriteLine($"{GetType()} Disappearing");
        }

        public int? OwnerPageKey
        {
            get => _ownerPageKey;
            set
            {
                Debug.WriteLine($"{GetType()} page key set to {value}");
                _ownerPageKey = value;
            }
        }

        public int? PageDepth { get; set; }

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
