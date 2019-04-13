using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FunctionZero.MvvmZero.Commanding
{

#if false
    public class DemoViewModel
    {
        public DemoViewModel()
        {
            this.DemoCommand = new Command<double>(async (x) => await CalculateSquareSlowly(x));
        }

        public ICommand DemoCommand { get; private set; }

        private async Task<double> CalculateSquareSlowly(double number)
        {
            await Task.Delay(2000);
            return number * number;
        }
    }

    public class DemoViewModel2
    {
        public DemoViewModel2()
        {
            this.DemoCommand = new Command(async () => await CalculateSquareSlowly(10));
        }

        public ICommand DemoCommand { get; private set; }

        private async Task<double> CalculateSquareSlowly(double number)
        {
            await Task.Delay(2000);
            return number * number;
        }
    }
#endif

    public class GuardCommandAsync : ICommand
    {
        private readonly IGuard _guard;
        private readonly Func<object, bool> _canExecute;
        private readonly Func<object, Task> _execute;
         
        public GuardCommandAsync(IGuard guard, Func<object, Task> execute)
            : this(guard, execute, (o) => true)
        {
        }
        
        public GuardCommandAsync(IGuard guard, Func<Task> execute)
            : this(guard, (o) => execute(), (o) => true)
        {
        }

        public GuardCommandAsync(IGuard guard, Func<Task> execute, Func<bool> canExecute)
            : this(guard, (o) => execute(), (o) => canExecute())
        {
        }

        public GuardCommandAsync(IGuard guard, Func<object, Task> execute, Func<object, bool> canExecute)
        {
            _guard = guard ?? throw new ArgumentNullException(nameof(guard));
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));

            _guard.GuardChanged += Guard_PropertyChanged;
        }

        ~GuardCommandAsync()
        {
            _guard.GuardChanged -= Guard_PropertyChanged;
        }

        private void Guard_PropertyChanged(object sender, GuardChangedEventArgs e)
        {
            this.ChangeCanExecute();
        }

        public bool CanExecute(object parameter)
        {
            return !_guard.IsGuardRaised && _canExecute(parameter);
        }

        /// <summary>
        /// Occurs when the target of the Command should reevaluate whether or not the Command can be executed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        public async void Execute(object parameter)
        {
            if (!_guard.IsGuardRaised)
            {
                try
                {
                    _guard.IsGuardRaised = true;
                    await _execute(parameter);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"GuardCommandAsync exception. Message: {ex.Message}");
                }
                finally
                {
                    _guard.IsGuardRaised = false;
                }
            }
        }



        public void ChangeCanExecute()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
