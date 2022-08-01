using System;

namespace Shaw.Srp.Model
{
    public class E21Plan : Plan
    {
        public E21Plan() : base("E-21 EZ3 3p-6p")
        {

        }

        public override float Cost(DateTime dateTime, float kWh, float monthlyKWhAccumulated)
        {
            if ((dateTime.Month >= 7)
                && (dateTime.Month <= 8))
            {
                return IsOnPeak(dateTime) ? (0.3444f * kWh) : (0.0853f * kWh);
            }
            else if ((dateTime.Month >= 5)
                && (dateTime.Month <= 10))
            {
                return IsOnPeak(dateTime) ? (0.2895f * kWh) : (0.0829f * kWh);
            }
            else
            {
                return IsOnPeak(dateTime) ? (0.1063f * kWh) : (0.0738f * kWh);
            }
        }

        public override bool IsOnPeak(DateTime dateTime)
        {
            if ((dateTime.DayOfWeek >= DayOfWeek.Monday)
                && (dateTime.DayOfWeek <= DayOfWeek.Friday)
                && !IsHoliday(dateTime))
            {
                if ((dateTime.Hour >= 15)
                    && (dateTime.Hour < 18))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
