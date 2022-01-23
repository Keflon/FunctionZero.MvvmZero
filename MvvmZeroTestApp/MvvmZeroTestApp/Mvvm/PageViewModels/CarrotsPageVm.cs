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
        public CommandZeroAsync PopModalCommand { get; }
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
                .SetExecuteAsync(NextCommandExecute)
                .SetName("Next")
                .Build(); 
            PopModalCommand= new CommandBuilder()
             .SetExecuteAsync(async ()=>await _pageService.PopAsync(true))
             .SetName("Pop Modal")
             .Build();
        }

        private async Task NextCommandExecute()
        {
            if (CanProceed == false)
                CanProceed = true;
            else
                await _pageService.PushPageAsync<ResultsPage, ResultsPageVm>((vm)=>((ResultsPageVm)vm).SetState("GO TEAM CARROT!!"), false, true, GetMvvmPageMode.Page);
        }

        public override void OnOwnerPageAppearing()
        {
            base.OnOwnerPageAppearing();
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