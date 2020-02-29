using FunctionZero.MvvmZero;
using FunctionZero.MvvmZero.Interfaces;
using FunctionZero.PageServiceZero;
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
        //public FlowPageServiceZero PageService { get; }

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

            IoCC.Register<IFlowPageServiceZero>(
                () =>
                {
                    var pageService = new FlowPageServiceZero(currentApplication, PageCreated);
                    pageService.RegisterTypeFactory((theType) => IoCC.GetInstance(theType));
                    return pageService;
                },
                Lifestyle.Singleton
            );
            /*
             TODO: Define FlowItem
             it contains a Page<TInput, TOutput>
             the page has 3 buttons
             each button has an action, that is passed in via TInput where TInput is tuple (Action AButtonAction, Action BButtonAction, Action CButtonAction)
             The FlowItem declares those Actions
             A FlowItem links its Page to child FlowItems, like currently a Page links itself to child Pages
             => the FlowItems encode the flow.
             How is state passed along a flow?
             How is input state passed from FlowItem to child?

              */
        }

        private void PageCreated(Page newPage)
        {
            Debug.WriteLine("Created a page");
        }
    }
}
