using FunctionZero.MvvmZero.Commanding;
using MvvmZeroTestApp.Mvvm.ViewModels;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MvvmZeroTestApp.Mvvm.PageViewModels
{
    public class RedPillPageVm : BaseVm
    {
        private bool _canProceed;

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

        public RedPillPageVm(/* TODO: Inject dependencies here */)
        {
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
                await App.Locator.PageService.PushPageAsync(Boilerplate.PageDefinitions.ResultsPage, new ResultsPageVm("GO TEAM RED!!"));
        }

        public override void OwnerPageAppearing(int? pageKey, int? pageDepth)
        {
            base.OwnerPageAppearing(pageKey, pageDepth);
            CanProceed = false;
        }

        //protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    base.OnPropertyChanged(propertyName);

        //    if(propertyName == nameof(CanProceed))
        //    {
        //        NextCommand.ChangeCanExecute();
        //    }
        //}
    }
}