using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using FunctionZero.MvvmZero.Commanding;
using MvvmZeroTestApp.Mvvm.Pages;
using Xamarin.Forms;

namespace MvvmZeroTestApp.Mvvm.PageViewModels
{
	public class SplashPageVm : BaseVm
	{
        public ICommand NextCommand { get; }
        public ICommand DummyAsyncCommand { get ; }
        public ICommand DummyCommand { get; }
		public SplashPageVm(/* TODO: Inject dependencies here */)
		{
            DummyAsyncCommand = new GuardCommandAsync(this, async () => Debug.WriteLine("FAKE!"));
            DummyCommand = new GuardCommand(this,  () => Debug.WriteLine("FAKE!"));
            NextCommand = new GuardCommandAsync(this, async ()=>await NextCommandExecute());
		}

        private async Task NextCommandExecute()
        {
            await Task.Delay(2000);
            await App.Locator.PageService.PushPageAsync(PageDefinitions.HomePage, new HomePageVm());
        }
    }
}