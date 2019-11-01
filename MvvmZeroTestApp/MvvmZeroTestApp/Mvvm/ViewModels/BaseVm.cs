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
        private double _scale;
        private double _anchorX;
        private double _anchorY;

        public BaseVm()
        {
            // Something sensible ...
            Scale = 1.0;
        }

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
            get => _rotation;
            set
            {
                if (value != _rotation)
                {
                    _rotation = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Scale
        {
            get => _scale;
            set
            {
                if (value != _scale)
                {
                    _scale = value;
                    OnPropertyChanged();
                }
            }
        }
        public double AnchorX
        {
            get => _anchorX;
            set
            {
                if (value != _anchorX)
                {
                    _anchorX = value;
                    OnPropertyChanged();
                }
            }
        }
        public double AnchorY
        {
            get => _anchorY;
            set
            {
                if (value != _anchorY)
                {
                    _anchorY = value;
                    OnPropertyChanged();
                }
            }
        }

        public override void OwnerPageAppearing(int? pageKey, int? pageDepth)
        {
            base.OwnerPageAppearing(pageKey, pageDepth);

            // HACK: Workaround for UWP BUG setting transform to 0.0 has no effect.
            //Rotation = 0.1;
            Rotation = 0.0;
            TranslationX = 0.1;
            TranslationX = 0.0;
            TranslationY = 0.1;
            TranslationY = 0.0;
            AnchorX = 0.0;
            AnchorY = 0.0;
            Scale = 1.0;
        }
        public override void OwnerPageDisappearing()
        {
            base.OwnerPageDisappearing();

            // iOS NavigationPage animates differently to UWP and Droid.
            if (Device.RuntimePlatform == Device.iOS)
            {
                Rotation = 0.0;
                TranslationX = 0.0;
                TranslationY = 0.0;
                Scale = 1.0;
            }
        }
    }
}
