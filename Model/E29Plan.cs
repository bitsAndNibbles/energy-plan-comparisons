using System;

namespace Shaw.Srp.Model
{
    public class E29Plan : Plan
    {
        public E29Plan() : base("E-29 EV TOU")
        {

        }

        public override float Cost(DateTime dateTime, float kWh, float monthlyKWhAccumulated)
        {
            if ((dateTime.Month >= 7)
                && (dateTime.Month <= 8))
            {
                if (IsSummerOnPeak(dateTime))
                {
                    return 0.2409f * kWh;
                }
                else if (IsSuperOffPeak(dateTime))
                {
                    return 0.0614f * kWh;
                }
                else
                {
                    return 0.0770f * kWh;
                }
            }
            else if ((dateTime.Month >= 5)
                && (dateTime.Month <= 10))
            {
                if (IsSummerOnPeak(dateTime))
                {
                    return 0.2094f * kWh;
                }
                else if (IsSuperOffPeak(dateTime))
                {
                    return 0.0611f * kWh;
                }
                else
                {
                    return 0.0765f * kWh;
                }
            }
            else
            {
                if (IsWinterOnPeak(dateTime))
                {
                    return 0.0951f * kWh;
                }
                else if (IsSuperOffPeak(dateTime))
                {
                    return 0.0575f * kWh;
                }
                else
                {
                    return 0.0737f * kWh;
                }
            }
        }

        public override bool IsOnPeak(DateTime dateTime)
        {
            return (IsSummerOnPeak(dateTime) || IsWinterOnPeak(dateTime));
        }

        private bool IsSummerOnPeak(DateTime dateTime)
        {
            if ((dateTime.DayOfWeek >= DayOfWeek.Monday)
                && (dateTime.DayOfWeek <= DayOfWeek.Friday)
                && !IsHoliday(dateTime))
            {
                if ((dateTime.Hour >= 14)
                    && (dateTime.Hour < 20))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsWinterOnPeak(DateTime dateTime)
        {
            if ((dateTime.DayOfWeek >= DayOfWeek.Monday)
                && (dateTime.DayOfWeek <= DayOfWeek.Friday)
                && !IsHoliday(dateTime))
            {
                if ((dateTime.Hour >= 5)
                    && (dateTime.Hour < 9))
                {
                    return true;
                }
                if ((dateTime.Hour >= 17)
                    && (dateTime.Hour < 21))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsSuperOffPeak(DateTime dateTime)
        {
            return ((dateTime.Hour == 23)
                || (dateTime.Hour < 5));
        }

    }
}
