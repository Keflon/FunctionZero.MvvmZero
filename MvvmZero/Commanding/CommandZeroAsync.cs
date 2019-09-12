/*
MIT License

Copyright(c) 2016 - 2019 Function Zero Ltd

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FunctionZero.MvvmZero.Commanding
{
    public class CommandZeroAsync : ICommand
    {
        private readonly IGuard _guard;
        private readonly Func<object, bool> _canExecute;
        private readonly Func<object, Task> _execute;

        internal class FalseGuard : IGuard
        {
            public bool IsGuardRaised { get => false; set { } }
            public event EventHandler<GuardChangedEventArgs> GuardChanged;
        }

        private static readonly IGuard _falseGuard = new FalseGuard();

        public CommandZeroAsync(Func<object, Task> execute)
            : this(_falseGuard, execute, (o) => true)
        {
        }

        public CommandZeroAsync(Func<Task> execute)
            : this(_falseGuard, (o) => execute(), (o) => true)
        {
        }

        public CommandZeroAsync(Func<Task> execute, Func<bool> canExecute)
            : this(_falseGuard, (o) => execute(), (o) => canExecute())
        {
        }

        public CommandZeroAsync(Func<Task> execute, Func<object, bool> canExecute)
            : this(_falseGuard, (o) => execute(), canExecute)
        {

        }

        public CommandZeroAsync(Func<object, Task> execute, Func<bool> canExecute)
            : this(_falseGuard, execute, (o) => canExecute())
        {
        }

        public CommandZeroAsync(Func<object, Task> execute, Func<object, bool> canExecute)
            : this(_falseGuard, execute, canExecute)
        {
        }



        public CommandZeroAsync(IGuard guard, Func<object, Task> execute)
            : this(guard, execute, (o) => true)
        {
        }

        public CommandZeroAsync(IGuard guard, Func<Task> execute)
            : this(guard, (o) => execute(), (o) => true)
        {
        }

        public CommandZeroAsync(IGuard guard, Func<Task> execute, Func<bool> canExecute)
            : this(guard, (o) => execute(), (o) => canExecute())
        {
        }

        public CommandZeroAsync(IGuard guard, Func<Task> execute, Func<object, bool> canExecute)
            : this(guard, (o) => execute(), canExecute)
        {
        }

        public CommandZeroAsync(IGuard guard, Func<object, Task> execute, Func<bool> canExecute)
            : this(guard, execute, (o) => canExecute())
        {
        }


        public CommandZeroAsync(IGuard guard, Func<object, Task> execute, Func<object, bool> canExecute)
        {
            _guard = guard == null ? _falseGuard : guard;
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute == null ? (o) => true : canExecute;

            _guard.GuardChanged += Guard_PropertyChanged;
        }

        ~CommandZeroAsync()
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
