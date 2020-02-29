using FunctionZero.MvvmZero.Commanding;
using FunctionZero.MvvmZero.Interfaces;
using MvvmZeroTestApp.Mvvm.Pages;
using MvvmZeroTestApp.Mvvm.ViewModels;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MvvmZeroTestApp.Mvvm.PageViewModels
{
    public class RedPillPageVm : BaseVm, IFlowPageZero<object, object>
    {
        private bool _canProceed;
        private IFlowPageServiceZero _pageService;

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

        public RedPillPageVm(IFlowPageServiceZero pageService)
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
                await _pageService.PushPageAsync<ResultsPage, ResultsPageVm>((vm)=>vm.SetState("GO TEAM RED!!"));
        }

        public override void OwnerPageAppearing(int? pageKey, int? pageDepth)
        {
            base.OwnerPageAppearing(pageKey, pageDepth);
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