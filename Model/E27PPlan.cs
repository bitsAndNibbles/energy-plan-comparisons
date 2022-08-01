using System;

namespace Shaw.Srp.Model
{
    public class E27PPlan : Plan
    {
        public E27PPlan() : base("E-27P Pilot - Demand Rate")
        {

        }

        public override float MonthlyServiceFee => 32.44f;

        public override float Cost(DateTime dateTime, float kWh, float monthlyKWhAccumulated)
        {
            if ((dateTime.Month >= 7)
                && (dateTime.Month <= 8))
            {
                if (IsSummerOnPeak(dateTime))
                {
                    return 0.0622f * kWh;
                }
                else
                {
                    return 0.0412f * kWh;
                }
            }
            else if ((dateTime.Month >= 5)
                && (dateTime.Month <= 10))
            {
                if (IsSummerOnPeak(dateTime))
                {
                    return 0.0462f * kWh;
                }
                else
                {
                    return 0.0360f * kWh;
                }
            }
            else
            {
                if (IsWinterOnPeak(dateTime))
                {
                    return 0.0410f * kWh;
                }
                else
                {
                    return 0.0370f * kWh;
                }
            }
        }

        private float DemandCharge(float peakKW, float first3, float next7, float allAddtl)
        {
            float charge = 0;

            if (peakKW > 10)
            {
                charge += allAddtl * (peakKW - 10);
                peakKW -= (peakKW - 10);
            }

            if (peakKW > 3)
            {
                charge += next7 * (peakKW - 3);
                peakKW -= (peakKW - 3);
            }

            charge += peakKW * first3;

            return charge;
        }

        public override float DemandCharge(int month, float peakKW)
        {
            if ((month == 7)  || (month == 8))
            {
                return DemandCharge(peakKW, 9.43f, 17.51f, 33.59f);
            }
            else if ((month >= 5) && (month <= 10))
            {
                return DemandCharge(peakKW, 7.89f, 14.37f, 27.28f);
            }
            else
            {
                return DemandCharge(peakKW, 3.49f, 5.58f, 9.57f);
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

    }
}
