using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FunctionZero.MvvmZero.Services
{
    public class LatchDelayMachine
    {
        private readonly TimeSpan _clockTimespan;
        private readonly int _clockTicksBeforeAction;
        private readonly Func<Task> _delayedActionAsync;
        private readonly Func<Task> _delayStartedActionAsync;
        private readonly Func<int, double, Task> _clockTickAsync;

        private int _counter;
        bool _timerIsRunning;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="millisecondClock"></param>
        /// <param name="clockTicksBeforeAction"></param>
        /// <param name="delayedActionAsync"></param>
        /// <param name="delayStartedActionAsync"></param>
        /// <param name="clockTickAsync"></param>
        public LatchDelayMachine(
            int millisecondClock,
            int clockTicksBeforeAction,
            Func<Task> delayedActionAsync,
            Func<Task> delayStartedActionAsync = null,
            Func<int, double, Task> clockTickAsync = null
            )
        {
            _clockTimespan = new TimeSpan(0, 0, 0, 0, millisecondClock);
            _clockTicksBeforeAction = clockTicksBeforeAction;
            _delayedActionAsync = delayedActionAsync ?? (async () => { });
            _delayStartedActionAsync = delayStartedActionAsync ?? (async () => { });
            _clockTickAsync = clockTickAsync ?? (async (count, progress) => { });
        }


        /// <summary>
        /// Calling this starts or resets the timer whose expiry raises delayedActionAsync
        /// </summary>
        /// <returns>False if an existing timer was reset; true if no timer was running and a new one was created.</returns>
        public bool Poke()
        {
            _counter = 0;

            if (!_timerIsRunning)
            {
                _timerIsRunning = true;
                _ = _delayStartedActionAsync?.Invoke();
                _ = _clockTickAsync(_counter, _counter / _clockTicksBeforeAction);
                Device.StartTimer(_clockTimespan, AreWeReadyToRock);
                return true;
            }
            return false;
        }

        private bool AreWeReadyToRock()
        {
            _counter++;
            _clockTickAsync(_counter, _counter / _clockTicksBeforeAction);

            if (_counter >= _clockTicksBeforeAction)
            {
                _timerIsRunning = false;
                _ = _delayedActionAsync.Invoke();
            }
            return _timerIsRunning;
        }
    }
}
