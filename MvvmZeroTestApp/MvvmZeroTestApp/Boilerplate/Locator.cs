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
            PageService.Register(PageDefinitions.RedPillPage, (vm) => new RedPillPage((RedPillPageVm)vm));
            PageService.Register(PageDefinitions.BluePillPage, (vm) => new BluePillPage((BluePillPageVm)vm));
            PageService.Register(PageDefinitions.ResultsPage, (vm) => new ResultsPage((ResultsPageVm)vm));
        }

        private void PageCreated(Page newPage)
        {
            Debug.WriteLine("Created a page");
        }
    }
}
