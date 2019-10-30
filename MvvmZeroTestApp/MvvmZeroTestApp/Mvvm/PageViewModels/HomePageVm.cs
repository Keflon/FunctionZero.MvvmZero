using FunctionZero.MvvmZero.Implementation;
using System;
using Xamarin.Forms;

namespace MvvmZeroTestApp.Mvvm.PageViewModels
{
    public class HomePageVm : BaseVm
    {
        public string FriendlyName => "Hello from the HomePageVm";

        public HomePageVm(/* TODO: Inject dependencies here */)
        {
        }
    }
}