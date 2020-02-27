using FunctionZero.MvvmZero.Interfaces;
using MvvmZeroTestApp.Boilerplate;
using MvvmZeroTestApp.Mvvm.ViewModels;
using System;
using Xamarin.Forms;

namespace MvvmZeroTestApp.Mvvm.PageViewModels
{
    public class ResultsPageVm : BaseVm, IFlowPageZero<string, int>
    {
        public string ResultsMessage { get; private set; }
        public ResultsPageVm()
        {
        }

        public void SetState(string state)
        {
            ResultsMessage = state;
        }

        public string GetState()
        {
            return ResultsMessage;
        }

        public int GetResult()
        {
            return 9;
        }
    }
}