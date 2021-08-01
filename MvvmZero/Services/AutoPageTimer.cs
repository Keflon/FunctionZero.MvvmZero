using System;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace FunctionZero.MvvmZero.Services
{
    public class AutoPageTimer
    {
        private readonly IHasOwnerPage _ownerPage;

        public int MillisecondInterval { get; }
        private readonly Action<object> _callback;
        private readonly Action<Exception> _exceptionHandler;
        public object State { get; }
        public bool IsActive => _ownerPage.IsOwnerPageVisible;

        public AutoPageTimer(IHasOwnerPage ownerPage, int millisecondInterval, Action<object> callback, Action<Exception> exceptionHandler = null, object state = null)
        {
            _ownerPage = ownerPage;
            MillisecondInterval = millisecondInterval;
            _callback = callback;
            _exceptionHandler = exceptionHandler;
            State = state;

            //_ownerPage.OwnerPageAppearing += OwnerPage_OwnerPageAppearing;
            _ownerPage.PropertyChanged += _ownerPage_PropertyChanged;
            if (_ownerPage.IsOwnerPageVisible)
                StartTimer();
        }

        private void _ownerPage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IHasOwnerPage.IsOwnerPageVisible))
            {
                var page = (IHasOwnerPage)sender;
                if (page.IsOwnerPageVisible)
                {
                    _lastPageToAppear = (IHasOwnerPage)sender;
                    StartTimer();
                }
            }
        }

        private static IHasOwnerPage _lastPageToAppear;

        private int _activeTimerCount = 0;
        //private void OwnerPage_OwnerPageAppearing(object sender, EventArgs e)
        //{
        //    _lastPageToAppear = (IHasOwnerPage)sender;
        //    StartTimer();
        //}

        private void StartTimer()
        {
            // Since we can't kill a running timer (e.g. in OwnerPageDisappearing) we count all the timers we start,
            // and use RawTimerCallback to kill all but the last one.
            _activeTimerCount++;
            Device.StartTimer(TimeSpan.FromMilliseconds(MillisecondInterval), RawTimerCallback);
        }

        private bool RawTimerCallback()
        {
            bool retval;

            if (_activeTimerCount > 1)
            {
                // Kill all but the last timer we started.
                retval = false;
            }
            else if ((IsActive == true) && (_lastPageToAppear != this._ownerPage))
            {
                // If INavigation.PopToRootAsync is called, the top page is not told it is disappearing,
                // so our IHasOwnerPage is not told its owner page is disappearing,
                // so the timer keeps running.
                // This stops the timer despite this XF bug. 
                // Note, PageServiceZero::PopToRootAsync *does* tell the top page is is disappearing.
                retval = false;
            }
            else
            {
                if (IsActive)
                {
                    try
                    {
                        _callback(State);
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            _exceptionHandler?.Invoke(ex);
                        }
                        catch { }
                    }
                }
                // Kill this timer if its ownerpage is not presented.
                retval = IsActive;
            }

            // If the current timer callback belongs to a timer we're killing, there is now one less active timer.
            if (retval == false)
                _activeTimerCount--;

            return retval;
        }
    }
}
