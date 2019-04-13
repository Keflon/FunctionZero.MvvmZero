using System;

namespace FunctionZero.MvvmZero.Commanding
{
    public interface IGuard
    {
        event EventHandler<GuardChangedEventArgs> GuardChanged;
        bool IsGuardRaised { get; set; }
    }

    public class GuardChangedEventArgs
    {
        public bool NewValue { get; }

        public GuardChangedEventArgs(bool newValue)
        {
            NewValue = newValue;
        }
    }
}
