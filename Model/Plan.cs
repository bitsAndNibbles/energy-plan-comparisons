using System;

namespace Shaw.Srp.Model
{
    public abstract class Plan
    {
        public Plan(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public virtual float MonthlyServiceFee => 20.0f;

        public abstract float Cost(DateTime dateTime, float kWh, float monthlyKWhAccumulated);

        public abstract bool IsOnPeak(DateTime dateTime);

        public virtual float DemandCharge(int month, float peakKW) => 0f;

        public bool IsHoliday(DateTime dateTime)
        {
            // New Year's Day (observed)
            if ((dateTime.Month == 12)
                && (dateTime.Day == 31)
                && (dateTime.DayOfWeek == DayOfWeek.Friday))
            {
                return true;
            }
            else if ((dateTime.Month == 1)
                && (dateTime.Day == 1)
                && (dateTime.DayOfWeek != DayOfWeek.Saturday)
                && (dateTime.DayOfWeek != DayOfWeek.Sunday))
            {
                return true;
            }
            else if ((dateTime.Month == 1)
                && (dateTime.Day == 2)
                && (dateTime.DayOfWeek == DayOfWeek.Monday))
            {
                return true;
            }

            // Memorial Day
            if ((dateTime.Month == 5)
                && (dateTime.DayOfWeek == DayOfWeek.Monday)
                && (dateTime.Day >= 25))
            {
                return true;
            }

            // Independence Day (observed)
            if ((dateTime.Month == 7)
                && (dateTime.Day == 3)
                && (dateTime.DayOfWeek == DayOfWeek.Friday))
            {
                return true;
            }
            else if ((dateTime.Month == 7)
                && (dateTime.Day == 4)
                && (dateTime.DayOfWeek != DayOfWeek.Saturday)
                && (dateTime.DayOfWeek != DayOfWeek.Sunday))
            {
                return true;
            }
            else if ((dateTime.Month == 7)
                && (dateTime.Day == 5)
                && (dateTime.DayOfWeek == DayOfWeek.Monday))
            {
                return true;
            }

            // Labor Day
            if ((dateTime.Month == 9)
                && (dateTime.DayOfWeek == DayOfWeek.Monday)
                && (dateTime.Day <= 7))
            {
                return true;
            }

            // Thanksgiving Day
            if ((dateTime.Month == 11)
                && (dateTime.DayOfWeek == DayOfWeek.Thursday)
                && (dateTime.Day >= 22)
                && (dateTime.Day <= 28))
            {
                return true;
            }

            // Christmas Day (observed)
            if ((dateTime.Month == 12)
                && (dateTime.Day == 24)
                && (dateTime.DayOfWeek == DayOfWeek.Friday))
            {
                return true;
            }
            else if ((dateTime.Month == 12)
                && (dateTime.Day == 25)
                && (dateTime.DayOfWeek != DayOfWeek.Saturday)
                && (dateTime.DayOfWeek != DayOfWeek.Sunday))
            {
                return true;
            }
            else if ((dateTime.Month == 12)
                && (dateTime.Day == 26)
                && (dateTime.DayOfWeek == DayOfWeek.Monday))
            {
                return true;
            }


            return false;
        }

    }
}
