using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using FunctionZero.MvvmZero;
using MvvmZeroTestApp.Mvvm.ViewModels;
using System;
using Xamarin.Forms;

namespace MvvmZeroTestApp.Mvvm.PageViewModels
{
    public class ResultsPageVm : BaseVm
    {
        public CommandZeroAsync PopToRootCommand { get; }

        public string ResultsMessage { get; private set; }

        public ResultsPageVm(IPageServiceZero pageService)
        {
            PopToRootCommand = new CommandBuilder()
                .SetName("Test PopToRoot")
                .SetExecute(
                () => pageService.PopToRootAsync()).Build();
        }

        public void SetState(string state)
        {
            ResultsMessage = state;
            this.OnPropertyChanged(nameof(ResultsMessage));
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