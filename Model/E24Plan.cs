using System;

namespace Shaw.Srp.Model
{
    public class E24Plan : Plan
    {
        public E24Plan() : base("E-24 M-Power")
        {

        }

        public override float Cost(DateTime dateTime, float kWh, float monthlyKWhAccumulated)
        {
            if ((dateTime.Month >= 7)
                && (dateTime.Month <= 8))
            {
                return 0.1185f * kWh;
            }
            else if ((dateTime.Month >= 5)
                && (dateTime.Month <= 10))
            {
                return 0.1114f * kWh;
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
