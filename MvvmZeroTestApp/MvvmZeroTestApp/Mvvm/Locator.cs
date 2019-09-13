//using System;
//using System.Collections.Generic;
//using System.Text;
//using FunctionZero.MvvmZero.Implementation;
//using MvvmZeroTestApp.Mvvm.Pages;
//using MvvmZeroTestApp.Mvvm.PageViewModels;
//using Xamarin.Forms;

//namespace MvvmZeroTestApp.Mvvm
//{
//    public class Locator
//    {
//        public PageServiceZero<PageDefinitions> PageService { get; }

//        public Locator()
//        {
//            PageService = new PageServiceZero<PageDefinitions>(Application.Current, PageCreateAction);

//            PageService.Register(PageDefinitions.SplashPage, viewModel => new SplashPage((SplashPageVm)viewModel));
//            PageService.Register(PageDefinitions.HomePage, viewModel => new HomePage((HomePageVm)viewModel));
//        }

//        private void PageCreateAction(Page newPage)
//        {

//        }
//    }
//}
