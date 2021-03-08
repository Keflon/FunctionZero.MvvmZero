using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MvvmZeroTestApp.Mvvm.Pages;
using MvvmZeroTestApp.Mvvm.ViewModels;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MvvmZeroTestApp.Mvvm.PageViewModels
{
    public class CarrotsPageVm : BaseVm
    {
        private bool _canProceed;
        private IPageServiceZero _pageService;

        public CommandZeroAsync NextCommand { get; }
        public bool CanProceed
        {
            get => _canProceed;
            set
            {
                if (value != _canProceed)
                {
                    _canProceed = value;
                    OnPropertyChanged();
                }
            }
        }

        public CarrotsPageVm(IPageServiceZero pageService)
        {
            _pageService = pageService;

            NextCommand = new CommandBuilder()
                //.SetCanExecute(() => CanProceed)
                .SetExecute(NextCommandExecute)
                .SetName("Next")
                .Build();
        }

        private async Task NextCommandExecute()
        {
            if (CanProceed == false)
                CanProceed = true;
            else
                await _pageService.PushPageAsync<ResultsPage>((vm)=>((ResultsPageVm)vm).SetState("GO TEAM CARROT!!"));
        }

        public override void OwnerPageAppearing()
        {
            base.OwnerPageAppearing();
            CanProceed = false;
        }

        public void SetState(object state)
        {
        }

        public object GetState()
        {
            return null;
        }

        public object GetResult()
        {
            return null;
        }
    }
}