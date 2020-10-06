using FunctionZero.MvvmZero;
using MvvmZeroTestApp.Mvvm.Pages;
using MvvmZeroTestApp.Mvvm.PageViewModels;
using MvvmZeroTestApp.Mvvm.ViewModels;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace MvvmZeroTestApp.Service
{
    public class Locator
    {
        public Container IoCC { get; }
        public Locator(Application currentApplication)
        {
            currentApplication.PageAppearing += Current_PageAppearing;
            currentApplication.PageDisappearing += Current_PageDisappearing;


            // Create the IoC container that will contain all our configurable classes ...
            IoCC = new Container();

            // Register the PageService ...
            IoCC.Register<IPageServiceZero>(() => new PageServiceZero(GetNavigationControl,
                                                                  (theType) => IoCC.GetInstance(theType)),
                                   Lifestyle.Singleton);

            // Register Pages ...
            IoCC.Register<HomePage>(Lifestyle.Transient);
            IoCC.Register<RedPillPage>(Lifestyle.Transient);
            IoCC.Register<ResultsPage>(Lifestyle.Transient);
            IoCC.Register<BluePillPage>(Lifestyle.Transient);

            // Register ViewModels ...
            IoCC.Register<HomePageVm>(Lifestyle.Transient);
            IoCC.Register<RedPillPageVm>(Lifestyle.Transient);
            IoCC.Register<ResultsPageVm>(Lifestyle.Transient);
            IoCC.Register<BluePillPageVm>(Lifestyle.Transient);

            // Register other things ...
            //IoCC.Register<IJarvisLogger, JarvisLogger>(Lifestyle.Singleton);

            var nav = new NavigationPage();
            //var nav = new NavigationPage(this.IoCC.GetInstance<IPageServiceZero>().MakePage<HomePage, HomePageVm>((vm) => vm.SetState(null)));
            //Application.Current.MainPage = nav;
            currentApplication.MainPage = nav;

            var homePage = this.IoCC.GetInstance<IPageServiceZero>().MakePage<HomePage, HomePageVm>((vm) => vm.SetState(null));

            this.IoCC.GetInstance<IPageServiceZero>().PushPageAsync(homePage, false);

            //_lastPageToAppear = _navPageHack.RootPage;


        }

        private void Current_PageDisappearing(object sender, Page e)
        {
            Debug.WriteLine($"Disappearing: {e.ToString()} ... {e.BindingContext}");
        }

        private void Current_PageAppearing(object sender, Page e)
        {
            Debug.WriteLine($"Appearing: {e.ToString()} ... {e.BindingContext}");
        }

        private INavigation GetNavigationControl()
        {
            return (INavigation)((NavigationPage)App.Current.MainPage).Navigation;
        }
    }
}
