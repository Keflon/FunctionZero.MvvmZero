using MvvmZeroTestApp.Boilerplate;
using MvvmZeroTestApp.Mvvm.PageViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmZeroTestApp
{
    public partial class App : Application
    {
        public Locator Locator { get; }

        public App()
        {
            InitializeComponent();

            Locator = new Locator(this);
            MainPage = Locator.PageService.PushPageAsync(PageDefinitions.HomePage, new HomePageVm()).Result;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
