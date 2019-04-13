using System;
using System.Threading.Tasks;
using MvvmZeroTestApp.Mvvm;
using MvvmZeroTestApp.Mvvm.Pages;
using MvvmZeroTestApp.Mvvm.PageViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MvvmZeroTestApp
{
    public partial class App : Application
    {

        public static Locator Locator { get; private set; }

        public App()
        {
            Locator = new Locator();

            InitializeComponent();

            Task<Page> newPage = Locator.PageService.PushPage(PageDefinitions.SplashPage, new SplashPageVm());

            //MainPage = new MainPage();
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
