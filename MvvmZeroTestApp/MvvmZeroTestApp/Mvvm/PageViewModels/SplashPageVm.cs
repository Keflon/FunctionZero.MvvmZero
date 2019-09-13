using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using FunctionZero.MvvmZero.Commanding;
using FunctionZero.MvvmZero.Implementation;
using MvvmZeroTestApp.Mvvm.Pages;
using Xamarin.Forms;

namespace MvvmZeroTestApp.Mvvm.PageViewModels
{
	public class SplashPageVm : BaseVm
	{
        public ICommand NextCommandAsync { get; }
        public ICommand DummyCommandAsync { get ; }
		public SplashPageVm(/* TODO: Inject dependencies here */)
		{
            DummyCommandAsync = new CommandZeroAsync(this, async () => Debug.WriteLine("FAKE!"));
            NextCommandAsync = new CommandZeroAsync(this, async () => await NextCommandExecute());
        }

        private async Task NextCommandExecute()
        {
            await Task.Delay(2000);
            await App.Locator.PageService.PushPageAsync(PageDefinitions.HomePage, new HomePageVm());
        }
    }
}