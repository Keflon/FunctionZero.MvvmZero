using System;
using System.Diagnostics;
using System.Windows.Input;

namespace FunctionZero.MvvmZero.Commanding
{
    public class GuardCommand : ICommand
    {
        private readonly IGuard _guard;
        private readonly Func<object, bool> _canExecute;
        private readonly Action<object> _execute;

        public GuardCommand(IGuard guard, Action<object> execute)
            : this(guard, execute, (o) => true)
        {
        }

        public GuardCommand(IGuard guard, Action execute)
            : this(guard, (o) => execute(), (o) => true)
        {
        }

        public GuardCommand(IGuard guard, Action execute, Func<bool> canExecute)
            : this(guard, (o) => execute(), (o) => canExecute())
        {
        }

        public GuardCommand(IGuard guard, Action<object> execute, Func<object, bool> canExecute)
        {
            _guard = guard ?? throw new ArgumentNullException(nameof(guard));
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));

            _guard.GuardChanged += Guard_PropertyChanged;
        }

        ~GuardCommand()
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

        public void Execute(object parameter)
        {
            if (!_guard.IsGuardRaised)
            {
                try
                {
                    _guard.IsGuardRaised = true;
                    _execute(parameter);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"GuardCommand exception. Message: {ex.Message}");
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
