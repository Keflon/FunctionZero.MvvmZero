using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace MvvmZeroTestApp.Mvvm.PageViewModels
{
    public abstract class BaseVm// : IGuard, INotifyPropertyChanged, IHasOwnerPage
    {
        //private readonly IGuard _guardImplementation;
        private int _ownerPageKey;
        public event PropertyChangedEventHandler PropertyChanged;

        public BaseVm()
        {
            //_guardImplementation = new BasicGuard();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OwnerPageAppearing(int pageKey, int? pageDepth)
        {
            OwnerPageKey = pageKey;
            Debug.WriteLine($"{this.GetType()} Appearing");
        }

        public virtual void OwnerPageDisappearing()
        {
            OwnerPageKey = -1;

            Debug.WriteLine($"{this.GetType()} Disappearing");
        }

        public int OwnerPageKey
        {
            get => _ownerPageKey;
            set
            {
                Debug.WriteLine($"{this.GetType()} page key set to {value}");
                _ownerPageKey = value;
            }
        }

        public int? PageDepth { get; set; }


    }
}
