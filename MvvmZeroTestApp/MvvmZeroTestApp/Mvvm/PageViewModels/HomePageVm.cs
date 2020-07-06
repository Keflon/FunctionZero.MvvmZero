using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MvvmZeroTestApp.Mvvm.Pages;
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

        private IPageServiceZero _pageService;

        public CommandZeroAsync RedPillCommand { get; }
        public CommandZeroAsync BluePillCommand { get; }

        public HomePageVm(IPageServiceZero pageService)
        {
            _pageService = pageService;

            RedPillCommand = new CommandBuilder().AddGuard(this).SetExecute(RedPillCommandExecute).SetName("Red Pill").Build();
            BluePillCommand = new CommandBuilder().AddGuard(this).SetExecute(BluePillCommandExecute).SetName("Blue Pill").Build();
        }

        private async Task RedPillCommandExecute()
        {
            await DoSomethingSilly();
            await _pageService.PushPageAsync<RedPillPage, RedPillPageVm>((vm) => vm.SetState(null));
        }

        private async Task DoSomethingSilly()
        {
            // Approximate simple harmonic motion.
            // MAGIC NUMBERS!!
            AnchorX = 0;
            AnchorY = 0;

            double time;
            double amplitude = 30;
            double damping = 0.995;

            Stopwatch s = new Stopwatch();
            s.Start();

            while (amplitude > 8)
            {
                time = s.ElapsedMilliseconds / 1000.0;
                Rotation = -Math.Cos(time * 5.0) * amplitude + 30.0;
                amplitude = amplitude * Math.Pow(damping, time);
                await Task.Delay(15);
            }

            s.Restart();
            // Approximate gravity. TODO: Acceleration.
            // MAGIC NUMBERS!!
            while (TranslationY < 500)
            {
                time = s.ElapsedMilliseconds / 1000.0;
                TranslationY = time * 500;
                await Task.Delay(15);
            }
            Rotation = 0.0;
            TranslationX = 0.1;
            TranslationX = 0.0;
            TranslationY = 0.1;
            TranslationY = 0.0;
            AnchorX = 0.0;
            AnchorY = 0.0;
            Scale = 1.0;
        }

        private async Task BluePillCommandExecute()
        {
            await DoSomethingElseSilly();
            await _pageService.PushPageAsync<BluePillPage, BluePillPageVm>((vm) => vm.SetState(null));
        }

        private async Task DoSomethingElseSilly()
        {
            // Approximate a cartoon balloon being deflated ...
            // MAGIC NUMBERS!!
            AnchorX = 0;
            AnchorY = 0;

            double duration = 3.0;
            Stopwatch s = new Stopwatch();
            s.Start();
            double time = 0.0;

            while (time < duration)
            {
                time = s.ElapsedMilliseconds / 1000.0;
                Rotation = (time *(time / duration) * (time / duration) * 720.0);
                AnchorX = Math.Sin(time*7.5);// * 10;
                AnchorY = Math.Cos(time*7.5);// * 10;
                Scale = 1 + Math.Sin((time / duration) * Math.PI/2.0*3.0);

                await Task.Delay(15);
            }
        }

        public void SetState(object state)
        {
        }

        public object GetState()
        {
            return null;
        }

        public object GetResult()
        {
            return null;
        }
    }
}