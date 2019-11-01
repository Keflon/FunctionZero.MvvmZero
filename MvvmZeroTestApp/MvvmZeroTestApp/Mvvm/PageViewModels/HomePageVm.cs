using FunctionZero.MvvmZero.Commanding;
using FunctionZero.MvvmZero.Implementation;
using FunctionZero.MvvmZero.Interfaces;
using MvvmZeroTestApp.Boilerplate;
using MvvmZeroTestApp.Mvvm.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MvvmZeroTestApp.Mvvm.PageViewModels
{
    public class HomePageVm : BaseVm
    {
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
            double damping = 0.995;
            while (amplitude > 8)
            {
                time = s.ElapsedMilliseconds / 1000.0;
                Rotation = -Math.Cos(time * 5.0) * amplitude + 30.0;
                amplitude = amplitude * Math.Pow(damping, time);
                await Task.Delay(15);
            }

            s.Restart();
            while (TranslationY < 500)
            {
                time = s.ElapsedMilliseconds / 1000.0;
                TranslationY = time * 500;
                await Task.Delay(15);
            }
        }




        private async Task BluePillCommandExecute()
        {
            await App.Locator.PageService.PushPageAsync(PageDefinitions.BluePillPage, new BluePillPageVm());
        }
    }
}