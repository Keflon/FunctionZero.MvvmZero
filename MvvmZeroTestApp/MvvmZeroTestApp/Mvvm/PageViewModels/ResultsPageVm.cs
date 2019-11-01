using MvvmZeroTestApp.Boilerplate;
using MvvmZeroTestApp.Mvvm.ViewModels;
using System;
using Xamarin.Forms;

namespace MvvmZeroTestApp.Mvvm.PageViewModels
{
    public class ResultsPageVm : BaseVm
    {
        public string ResultsMessage { get; }
        public ResultsPageVm(string messageToDisplay)
        {
            ResultsMessage = messageToDisplay;
        }
    }
}