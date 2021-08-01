using FunctionZero.MvvmZero;
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

            base.AddPageTimer(10000, timerCallback, null, "hello");
        }

        private void timerCallback(object obj)
        {
            Debug.WriteLine($"{this} beep");
        }

        public double TranslationX
        {
            get => _translationX;
            set => SetProperty(ref _translationX, value);
        }
        public double TranslationY
        {
            get => _translationY;
            set => SetProperty(ref _translationY, value);
        }

        public double Rotation
        {
            get => _rotation;
            set => SetProperty(ref _rotation, value);
        }
        public double Scale
        {
            get => _scale;
            set => SetProperty(ref _scale, value);
        }
        public double AnchorX
        {
            get => _anchorX;
            set => SetProperty(ref _anchorX, value);
        }
        public double AnchorY
        {
            get => _anchorY;
            set => SetProperty(ref _anchorY, value);
        }

        public override void OnOwnerPageAppearing()
        {
            base.OnOwnerPageAppearing();

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
        public override void OnOwnerPageDisappearing()
        {
            base.OnOwnerPageDisappearing();

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
