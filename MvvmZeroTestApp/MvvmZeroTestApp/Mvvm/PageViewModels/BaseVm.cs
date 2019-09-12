using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using FunctionZero.MvvmZero.Commanding;
using FunctionZero.MvvmZero.Interfaces;
using MvvmZeroTestApp.Annotations;
using MvvmZeroTestApp.Mvvm.Pages;

namespace MvvmZeroTestApp.Mvvm.PageViewModels
{
    public abstract class BaseVm : IGuard, INotifyPropertyChanged, IHasOwnerPage<PageDefinitions>
    {
        private readonly IGuard _guardImplementation;
        private PageDefinitions _ownerPageKey;
        public event PropertyChangedEventHandler PropertyChanged;

        public BaseVm()
        {
            _guardImplementation = new BasicGuard();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OwnerPageAppearing(PageDefinitions pageKey, int? pageDepth)
        {
            OwnerPageKey = pageKey;
            Debug.WriteLine($"{this.GetType()} Appearing");
        }

        public void OwnerPageDisappearing()
        {
            OwnerPageKey = PageDefinitions.None;

            Debug.WriteLine($"{this.GetType()} Disappearing");
        }

        public PageDefinitions OwnerPageKey
        {
            get => _ownerPageKey;
            set
            {
                Debug.WriteLine($"{this.GetType()} page key set to {value}");
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
