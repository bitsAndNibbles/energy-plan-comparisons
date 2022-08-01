using System;

namespace Shaw.Srp.Model
{
    public class HourlyEntry
    {
        public HourlyEntry(DateTime dateTime, float kWh, float cost)
        {
            DateTime = dateTime;
            KWh = kWh;
            Cost = cost;
        }

        public DateTime DateTime { get; }

        public float KWh { get; }

        public float Cost { get; }

    }
}
