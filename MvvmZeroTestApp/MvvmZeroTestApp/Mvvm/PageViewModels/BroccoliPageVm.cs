using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MvvmZeroTestApp.Mvvm.Pages;
using MvvmZeroTestApp.Mvvm.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MvvmZeroTestApp.Mvvm.PageViewModels
{
    public class BroccoliPageVm : BaseVm
    {
        private int _puzzleProgress;
        private double _zeroWidth;
        private double _oneWidth;
        private double _twoWidth;
        private IPageServiceZero _pageService;

        public ICommandZero ZeroCommand { get; }
        public ICommandZero OneCommand { get; }
        public ICommandZero TwoCommand { get; }
        
        public ICommandZero ResetCommand { get; }
        public int PuzzleProgress
        {
            get => _puzzleProgress;
            set
            {
                if(value != _puzzleProgress)
                {
                    _puzzleProgress = value;
                    OnPropertyChanged();
                }
            }
        }
        public BroccoliPageVm(IPageServiceZero pageService)
        {
            _pageService = pageService;
            ZeroCommand = new CommandBuilder().AddGuard(this).SetCanExecute(()=>PuzzleProgress == 0).SetExecuteAsync(AnyCommandExecuteAsync).SetName("Zero").Build();
            OneCommand = new CommandBuilder().AddGuard(this).SetCanExecute(() => PuzzleProgress == 1).SetExecuteAsync(AnyCommandExecuteAsync).SetName("One").Build();
            TwoCommand = new CommandBuilder().AddGuard(this).SetCanExecute(() => PuzzleProgress == 2).SetExecuteAsync(AnyCommandExecuteAsync).SetName("Two").Build();

            ResetCommand = new CommandBuilder().AddObservedProperty(this, nameof(PuzzleProgress)).SetExecute(TestCommandExecute).SetCanExecute(TestCommandCanExecute).SetName("RESET").Build();


            _stopwatch = new Stopwatch();
        }

        private bool TestCommandCanExecute(object arg)
        {
            return PuzzleProgress != 0;
        }

        private void TestCommandExecute()
        {
            PuzzleProgress = 0;
        }

        private async Task AnyCommandExecuteAsync()
        {
            PuzzleProgress++;
            if(PuzzleProgress == 3)
                await _pageService.PushPageAsync<ResultsPage, ResultsPageVm>((vm)=>((ResultsPageVm)vm).SetState("GO TEAM BROCCOLI!!"), false, true);
        }

        public double ZeroWidth
        {
            get => _zeroWidth;
            set
            {
                if (value != _zeroWidth)
                {
                    _zeroWidth = value;
                    OnPropertyChanged();
                }
            }
        }
        public double OneWidth
        {
            get => _oneWidth;
            set
            {
                if (value != _oneWidth)
                {
                    _oneWidth = value;
                    OnPropertyChanged();
                }
            }
        }
        public double TwoWidth
        {
            get => _twoWidth;
            set
            {
                if (value != _twoWidth)
                {
                    _twoWidth = value;
                    OnPropertyChanged();
                }
            }
        }

        Stopwatch _stopwatch;
        public override void OnOwnerPageAppearing()
        {
            base.OnOwnerPageAppearing();

            PuzzleProgress = 0;
            // BindingContext may not yet be set, so force a ChangeCanExecute ...
            ((CommandZeroAsync)ZeroCommand).ChangeCanExecute();
            _stopwatch = Stopwatch.StartNew();
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 30), TimerCallback);
        }

        private bool TimerCallback()
        {
            // Make the button's widths follow a sine wave with offsets ...
            double rate = 200.0;
            double offset = 140.0;
            long time = _stopwatch.ElapsedMilliseconds;
            ZeroWidth = 150 + Math.Sin(time / rate) * 50;
            OneWidth = 150 + Math.Sin((time + offset) / rate) * 50;
            TwoWidth = 150 + Math.Sin((time + offset + offset) / rate) * 50;

            // return false to stop the timer.
            return _stopwatch.IsRunning;
        }

        public override void OnOwnerPageDisappearing()
        {
            _stopwatch.Stop();
            PuzzleProgress = 0;

            base.OnOwnerPageDisappearing();
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