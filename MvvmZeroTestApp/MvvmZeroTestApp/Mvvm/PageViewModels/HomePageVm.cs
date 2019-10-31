using FunctionZero.MvvmZero.Commanding;
using FunctionZero.MvvmZero.Implementation;
using MvvmZeroTestApp.Boilerplate;
using System;
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
            await App.Locator.PageService.PushPageAsync(PageDefinitions.HomePage, new HomePageVm());
            //await App.Locator.PageService.PushPageAsync(PageDefinitions.RedPillPage, new RedPillPageVm());
        }
        private async Task BluePillCommandExecute()
        {
            await App.Locator.PageService.PushPageAsync(PageDefinitions.BluePillPage, new BluePillPageVm());
        }
    }
}