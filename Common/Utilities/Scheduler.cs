using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Common.Utilities
{
    public static class Scheduler
    {
        static Scheduler()
        {
            // Do nothing?
        }
        
        public static Timer GetTimerMillis(double millis)
        {
            Timer myTimer = new Timer();
            myTimer.Interval = (millis > 0) ? millis : 1000;
            myTimer.AutoReset = false;
            myTimer.Enabled = true;
            return myTimer;
        }

        public static Timer GetIntervalTimerMillis(double millis)
        {
            Timer myTimer = new Timer();
            myTimer.Interval = (millis > 0) ? millis : 1000;
            myTimer.AutoReset = true;
            myTimer.Enabled = true;
            return myTimer;
        }

        public static Timer GetTimerMinutes(double minutes)
        {
            return GetTimerMillis(minutes * 60 * 1000);
        }

        public static Timer GetIntervalTimerMinutes(double minutes)
        {
            return GetIntervalTimerMillis(minutes * 60 * 1000);
        }

        public static double MillisUntil(DateTime time)
        {
            double currentMillis = DateTime.Now.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            double futureMillis = time.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            return (futureMillis > currentMillis)? futureMillis - currentMillis : 0;
        }
    }
}
