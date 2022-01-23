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
        private readonly Action _delayKilledAction;
        private int _counter;
        bool _timerIsRunning;

        /// <summary>
        /// We can't stop a timer directly, so this bool is used to flag that a kill is requested
        /// </summary>
        private bool _killPokeRequested = false;


        // TODO: Either add a 'busy' flag and a BusyChanged event (e.g. to manage a 'Busy' flag in the consumer), or
        // TODO: Add a bool to delayedAction describing whether the delayedAction was killed.
        // TODO: Add a state-object.
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
            Func<int, double, bool> clockTick = null,
            Action delayKilledAction = null
            )
        {
            _clockTimespan = new TimeSpan(0, 0, 0, 0, millisecondClock);
            _clockTicksBeforeAction = clockTicksBeforeAction;
            _delayedAction = delayedAction ?? (() => { });
            _delayStartedAction = delayStartedAction ?? (() => true);
            _clockTick = clockTick ?? ((count, progress) => true);
            _delayKilledAction = delayKilledAction ?? (() => { });
        }


        /// <summary>
        /// Calling this starts or resets the timer whose expiry raises delayedActionAsync
        /// </summary>
        /// <returns>False if an existing timer was reset; true if no timer was running and a new one was created.</returns>
        public bool Poke()
        {
            // If a request is pending to kill a Poke, revoke it.
            _killPokeRequested = false;

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

        /// <summary>
        /// Prevents a poked timer from continuing
        /// Note: This 'requests' a timer to stop, therefore if the timer
        /// is running, _timerIsRunning will remain true until the next timer callback.
        /// </summary>
        /// <returns>True if there was a timer running</returns>
        public bool KillPoke()
        {
            if (_timerIsRunning)
                _killPokeRequested = true;

            return _timerIsRunning;
        }

        private bool AreWeReadyToRock()
        {
            // If there's a pending request to kill the timer, cancel the request and kill the timer.
            if (_killPokeRequested == true)
            {
                _killPokeRequested = false;
                _timerIsRunning = false;
                _delayKilledAction();
                return false; 
            }

            _counter++;

            // If the clock callback returns false, kill the timer.
            if (_clockTick(_counter, (double)_counter / (double)_clockTicksBeforeAction) == false)
            {
                _timerIsRunning = false;
            }
            // Otherwise, if it's time to do the delayed action ...
            else if (_counter >= _clockTicksBeforeAction)
            {
                _timerIsRunning = false;
                _delayedAction();
                // This timer must die. _delayedAction may have started a new timer, setting _timerIsRunning to true
                // So don't return _timerIsRunning, return false!
                return false;
            }
            return _timerIsRunning;
        }
    }
}
