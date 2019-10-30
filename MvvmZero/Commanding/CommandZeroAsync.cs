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
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FunctionZero.MvvmZero.Commanding
{
    public class CommandZeroAsync : ICommand
    {
        private readonly IEnumerable<IGuard> _guardList;
        private readonly Func<object, bool> _canExecute;
        private readonly Func<object, Task> _execute;
        private int _raisedGuardCount;
        /// <summary>
        /// Occurs when the target of the Command should reevaluate whether or not the Command can be executed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        public Func<string> NameGetter { get; }
        public string FriendlyName => NameGetter();

        public CommandZeroAsync(IEnumerable<IGuard> guardList, Func<object, Task> execute, Func<object, bool> canExecute, Func<string> nameGetter)
        {
            _guardList = guardList ?? throw new ArgumentNullException(nameof(guardList));
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? ((o) => true);
            NameGetter = nameGetter ?? (() => string.Empty);
             
            foreach (var guard in _guardList)
            {
                if (guard.IsGuardRaised)
                    _raisedGuardCount++;
                guard.GuardChanged += Guard_GuardChanged;
            }
        }
        public bool CanExecute(object parameter)
        {
            return (_raisedGuardCount == 0) && _canExecute(parameter);
        }

        private void Guard_GuardChanged(object sender, GuardChangedEventArgs e)
        {
            if (e.NewValue == true)
            {
                _raisedGuardCount++;
                if (_raisedGuardCount == 1)
                    this.ChangeCanExecute();
            }
            else
            {
                _raisedGuardCount--;
                if (_raisedGuardCount == 0)
                    this.ChangeCanExecute();
            }
        }

        ~CommandZeroAsync()
        {
            foreach (var guard in _guardList)
                guard.GuardChanged -= Guard_GuardChanged;
        }

        /// <summary>
        /// Used by ICommand observers.
        /// </summary>
        /// <param name="parameter"></param>
        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        /// <summary>
        /// Used by grownups.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteAsync(object parameter)
        {
            if (_raisedGuardCount == 0)
            {
                try
                {
                    foreach (var guard in _guardList)
                        guard.IsGuardRaised = true;
                    await _execute(parameter);
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"GuardCommandAsync exception. Message: {ex.Message}");
                }
                finally
                {
                    foreach (var guard in _guardList)
                        guard.IsGuardRaised = false;
                }
            }
            return false;
        }

        public void ChangeCanExecute()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
