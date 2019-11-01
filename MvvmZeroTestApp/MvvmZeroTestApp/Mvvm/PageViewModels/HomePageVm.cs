using FunctionZero.MvvmZero.Commanding;
using FunctionZero.MvvmZero.Implementation;
using FunctionZero.MvvmZero.Interfaces;
using MvvmZeroTestApp.Boilerplate;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MvvmZeroTestApp.Mvvm.PageViewModels
{
    public class HomePageVm : BaseVm
    {
        private double _translationX;
        private double _translationY;
        private double _rotation;

        public string FriendlyName => "Hello from the HomePageVm";

        public CommandZeroAsync RedPillCommand { get; }
        public CommandZeroAsync BluePillCommand { get; }

        public HomePageVm(/* TODO: Inject dependencies here */)
        {
            RedPillCommand = new CommandBuilder().AddGuard(this).SetExecute(RedPillCommandExecute).SetName("Red Pill").Build();
            BluePillCommand = new CommandBuilder().AddGuard(this).SetExecute(BluePillCommandExecute).SetName("Blue Pill").Build();
        }

        private async Task RedPillCommandExecute()
        {
            await DoSomethingSilly();

            await App.Locator.PageService.PushPageAsync(PageDefinitions.RedPillPage, new RedPillPageVm());
        }

        private async Task DoSomethingSilly()
        {
            //Simple harmonic motion.
            Stopwatch s = new Stopwatch();
            s.Start();
            double time = 0.0;
            double amplitude = 30;
            double damping = 0.99;
            while (amplitude > 8)
            {
                Rotation = -Math.Cos(time) * amplitude + 30.0;
                time = s.ElapsedMilliseconds * 5.0 / 1000.0;
                amplitude = amplitude * damping;
                await Task.Delay(30);
            }

            s.Restart();
            while (_translationY < 500)
            {
                time = s.ElapsedMilliseconds / 1000.0;
                TranslationY = time * 500;
                await Task.Delay(30);
            }
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
            get => _rotation; set
            {
                if (value != _rotation)
                {
                    _rotation = value;
                    OnPropertyChanged();
                }
            }
        }

        private async Task BluePillCommandExecute()
        {
            await App.Locator.PageService.PushPageAsync(PageDefinitions.BluePillPage, new BluePillPageVm());
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