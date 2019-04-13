using System;

namespace FunctionZero.MvvmZero.Commanding
{
    public class BasicGuard : IGuard
    {
        private bool _isGuardRaised;
        public event EventHandler<GuardChangedEventArgs> GuardChanged;

        public bool IsGuardRaised
        {
            get => _isGuardRaised;
            set
            {
                if (_isGuardRaised != value)
                {
                    _isGuardRaised = value;
                    GuardChanged?.Invoke(this, new GuardChangedEventArgs(value));
                }
            }
        }
    }
}
