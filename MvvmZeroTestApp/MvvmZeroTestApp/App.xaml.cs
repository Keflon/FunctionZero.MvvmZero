﻿using MvvmZeroTestApp.Boilerplate;
using MvvmZeroTestApp.Mvvm.PageViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmZeroTestApp
{
    public partial class App : Application
    {
        public static Locator Locator { get; private set; }

        public App()
        {
            InitializeComponent();

            // Initialise the Locator instance used to orchestrate our pages ...
            Locator = new Locator(this);

            // Ask the Locator to prepare and present our first page ...
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
