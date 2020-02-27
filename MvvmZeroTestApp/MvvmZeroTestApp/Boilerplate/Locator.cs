using FunctionZero.MvvmZero;
using FunctionZero.PageServiceZero;
using MvvmZeroTestApp.Mvvm.Pages;
using MvvmZeroTestApp.Mvvm.PageViewModels;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace MvvmZeroTestApp.Boilerplate
{
    public class Locator
    {
        public Container IoCC { get; }
        public FlowPageServiceZero PageService { get; }

        public Locator(Application currentApplication)
        {
            // Create the IoC container that will contain all our configurable classes ...
            IoCC = new Container();

            IoCC.Register<HomePage>(Lifestyle.Transient);
            IoCC.Register<HomePageVm>(Lifestyle.Transient);
            IoCC.Register<RedPillPage>(Lifestyle.Transient);
            IoCC.Register<RedPillPageVm>(Lifestyle.Transient);
            IoCC.Register<BluePillPage>(Lifestyle.Transient);
            IoCC.Register<ResultsPage>(Lifestyle.Transient);
            IoCC.Register<ResultsPageVm>(Lifestyle.Transient);



            //IoCC.Register<IJarvisLogger, JarvisLogger>(Lifestyle.Singleton);
            //IoCC.Register<IPageServiceZero<PageDefinitions>>(() => new PageServiceZero<PageDefinitions>(currentApplication, PageCreated), Lifestyle.Singleton);


            PageService = new FlowPageServiceZero(currentApplication, PageCreated);

            PageService.RegisterTypeFactory((theType) => IoCC.GetInstance(theType));

            //PageService.Register(PageDefinitions.HomePage, (vm) => new HomePage((HomePageVm)vm));
            //PageService.Register(PageDefinitions.RedPillPage, (vm) => new RedPillPage((RedPillPageVm)vm));
            //PageService.Register(PageDefinitions.BluePillPage, (vm) => new BluePillPage((BluePillPageVm)vm));
            //PageService.Register(PageDefinitions.ResultsPage, (vm) => new ResultsPage((ResultsPageVm)vm));
        }

        private void PageCreated(Page newPage)
        {
            Debug.WriteLine("Created a page");
        }
    }
}
