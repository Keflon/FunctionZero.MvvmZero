using FunctionZero.PageServiceZero;
using MvvmZeroTestApp.Mvvm.Pages;
using MvvmZeroTestApp.Mvvm.PageViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace MvvmZeroTestApp.Boilerplate
{
    public class Locator
    {
        public PageServiceZero<PageDefinitions> PageService { get; }

        public Locator(Application currentApplication)
        {
            PageService = new PageServiceZero<PageDefinitions>(currentApplication, PageCreated);

            PageService.Register(PageDefinitions.HomePage, (vm) => new HomePage((HomePageVm)vm));
            //PageService.Register(PageDefinitions.HomeOnePage, (vm) => new HomeOnePage((HomeOnePageVm)vm));
            //PageService.Register(PageDefinitions.HomeTwoPage, (vm) => new HomeTwoPage((HomeTwoPageVm)vm));
        }

        private void PageCreated(Page newPage)
        {
            Debug.WriteLine("Created a page");
        }
    }
}
