using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FunctionZero.MvvmZero.Services
{
    public class LatchDelayMachine
    {
        private readonly TimeSpan _clockTimespan;
        private readonly int _clockTicksBeforeAction;
        private readonly Action _delayedAction;
        private readonly Func<bool> _delayStartedAction;
        private readonly Func<int, double, bool> _clockTick;

        private int _counter;
        bool _timerIsRunning;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="millisecondClock"></param>
        /// <param name="clockTicksBeforeAction"></param>
        /// <param name="delayedAction"></param>
        /// <param name="delayStartedAction"></param>
        /// <param name="clockTick"></param>
        public LatchDelayMachine(
            int millisecondClock,
            int clockTicksBeforeAction,
            Action delayedAction,
            Func<bool> delayStartedAction = null,
            Func<int, double, bool> clockTick = null
            )
        {
            _clockTimespan = new TimeSpan(0, 0, 0, 0, millisecondClock);
            _clockTicksBeforeAction = clockTicksBeforeAction;
            _delayedAction = delayedAction ?? (() => { });
            _delayStartedAction = delayStartedAction ?? (() => true);
            _clockTick = clockTick ?? ((count, progress) => true);
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
                if (_delayStartedAction() == false)
                    return false;

                if (_clockTick(_counter, _counter / _clockTicksBeforeAction) == false)
                    return false;

                _timerIsRunning = true;

                Device.StartTimer(_clockTimespan, AreWeReadyToRock);
                return true;
            }
            return false;
        }

        private bool AreWeReadyToRock()
        {
            _counter++;
            if (_clockTick(_counter, (double)_counter / (double)_clockTicksBeforeAction) == false)
            {
                _timerIsRunning = false;
            }
            else if (_counter >= _clockTicksBeforeAction)
            {
                _timerIsRunning = false;
                // This timer must die. _delayedAction may now start a new timer, setting _timerIsRunning to true. 
                _delayedAction();
                return false;
            }
            return _timerIsRunning;
        }
    }
}
