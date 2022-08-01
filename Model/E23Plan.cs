using System;

namespace Shaw.Srp.Model
{
    public class E23Plan : Plan
    {
        public E23Plan() : base("E-23 Standard")
        {

        }

        public override float Cost(DateTime dateTime, float kWh, float monthlyKWhAccumulated)
        {
            if ((dateTime.Month >= 7)
                && (dateTime.Month <= 8))
            {
                if (monthlyKWhAccumulated >= 2001)
                {
                    return 0.1270f * kWh;
                }
                else
                {
                    return 0.1157f * kWh;
                }
            }
            else if ((dateTime.Month >= 5)
                && (dateTime.Month <= 10))
            {
                if (monthlyKWhAccumulated >= 2001)
                {
                    return 0.1134f * kWh;
                }
                else
                {
                    return 0.1091f * kWh;
                }
            }
            else
            {
                return 0.0782f * kWh;
            }
        }

        public override bool IsOnPeak(DateTime dateTime)
        {
            return false;
        }

    }
}
