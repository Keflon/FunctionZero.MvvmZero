using FunctionZero.MvvmZero.Implementation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace MvvmZeroTestApp.Mvvm.ViewModels
{
    public class BaseVm : MvvmZeroBaseVm
    {
        private double _translationX;
        private double _translationY;
        private double _rotation;

        public double TranslationX
        {
            get => _translationX; set
            {
                if (value != _translationX)
                {
                    _translationX = value;
                    OnPropertyChanged();
                }
            }
        }
        public double TranslationY
        {
            get => _translationY;
            set
            {
                if (value != _translationY)
                {
                    _translationY = value;
                    OnPropertyChanged();
                    Debug.WriteLine($"TranslationY:{TranslationY}");
                }
            }
        }
        public double Rotation
        {
            get => _rotation; set
            {
                if (value != _rotation)
                {
                    _rotation = value;
                    OnPropertyChanged();
                }
            }
        }

        public override void OwnerPageAppearing(int? pageKey, int? pageDepth)
        {
            base.OwnerPageAppearing(pageKey, pageDepth);

            // HACK: Workaround for UWP BUG setting transform to 0.0 has no effect.
            Rotation = 0.1;
            Rotation = 0.0;
            TranslationX = 0.1;
            TranslationX = 0.0;
            TranslationY = 0.1;
            TranslationY = 0.0;
        }
        public override void OwnerPageDisappearing()
        {
            base.OwnerPageDisappearing();

            if (Device.RuntimePlatform == Device.iOS)
            {
                Rotation = 0.0;
                TranslationX = 0.0;
                TranslationY = 0.0;
            }
        }
    }
}
