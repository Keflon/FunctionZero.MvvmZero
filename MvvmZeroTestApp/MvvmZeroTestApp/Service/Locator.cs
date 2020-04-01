using FunctionZero.MvvmZero;
using FunctionZero.MvvmZero.Interfaces;
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
            // Create the IoC container that will contain all our configurable classes ...
            IoCC = new Container();

            // Register the PageService ...
            IoCC.Register<IPageServiceZero>(() => new PageServiceZero(currentApplication,
                                                                              (theType) => IoCC.GetInstance(theType),
                                                                              PageCreated),
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

            // Register other things ...
            //IoCC.Register<IJarvisLogger, JarvisLogger>(Lifestyle.Singleton);
        }

        private void PageCreated(Page newPage)
        {
            Debug.WriteLine($"Created a page of type {newPage.GetType()}");
        }
    }
}
