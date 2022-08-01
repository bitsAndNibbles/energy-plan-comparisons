using Shaw.Srp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shaw.Srp.Service
{
    internal class UsageDataReader : IDisposable
    {
        private readonly StreamReader _reader;

        private const string FIRST_LINE = "Usage Date,Hour,kWh,Cost";
        private const string FIRST_LINE_ALT = "Usage date,Interval,Total kWh";

        internal UsageDataReader(Stream inputStream)
        {
            _reader = new StreamReader(inputStream);
        }

        internal async Task<List<HourlyEntry>> ReadHourlyAsync()
        {
            var entries = new List<HourlyEntry>();

            var inputLine = await _reader.ReadLineAsync().ConfigureAwait(false);
            if (!FIRST_LINE.Equals(inputLine) && !FIRST_LINE_ALT.Equals(inputLine))
            {
                throw new ApplicationException($"Fist line must be: {FIRST_LINE} *OR* {FIRST_LINE_ALT}");
            }

            bool foundStartMonth = false;

            // add some kWh corresponding to EV charging, assuming about 32 miles driven per day.
            float milesDrivenDaily = 12000f/365f;
            float referenceMilesChargedPer32AHour = 22.0f;
            float referenceVoltage = 230.0f;
            float amps = 16f;
            float volts = 230.0f;
            float WtoKW = 0.001f;

            float evChargeHours = milesDrivenDaily / referenceMilesChargedPer32AHour * 32.0f * referenceVoltage / amps / volts;
            if (volts <= 130)
            {
                evChargeHours *= 2f;
            }
            Console.WriteLine($"EV Charge Hours: {evChargeHours}");
            float evChargeKW = amps * volts * WtoKW;

            HourlyEntry entry = null;
            while (!_reader.EndOfStream)
            {
                inputLine = await _reader.ReadLineAsync().ConfigureAwait(false);

                var entryParts = inputLine.Split(new char[] { ',' }, 4, StringSplitOptions.None);

                DateTime dateTime = ParseDateAndTime(entryParts[0], entryParts[1]);
                float kWh = float.Parse(entryParts[2]);
                float cost;
                if (entryParts.Length >= 4)
                {
                    cost = float.Parse(entryParts[3].Substring(1));
                }
                else
                {
                    cost = 0f;
                }

                float evChargeThisHourPercentage = Math.Max(0f, Math.Min(1f, evChargeHours - dateTime.Hour));
                kWh += evChargeThisHourPercentage * evChargeKW;

                entry = new HourlyEntry(dateTime, kWh, cost);

                if (foundStartMonth || dateTime.Day == 1)
                {
                    foundStartMonth = true;
                    entries.Add(entry);
                }
            }

            int daysInLastEntryMonth = DateTime.DaysInMonth(entry.DateTime.Year, entry.DateTime.Month);
            if (entry.DateTime.Day < daysInLastEntryMonth)
            {
                // trim out this month's entries since it is an incomplete month
                int exclusionStartIndex = entries.FindIndex(match =>
                    (match.DateTime.Year == entry.DateTime.Year)
                    && (match.DateTime.Month == entry.DateTime.Month));
                entries.RemoveRange(exclusionStartIndex, entries.Count - exclusionStartIndex);
            }

            Console.WriteLine($"First entry: {entries[0].DateTime}");
            Console.WriteLine($"Last entry: {entries.Last().DateTime}");


            return entries;
        }

        private static DateTime ParseDateAndTime(string date, string time)
        {
            return DateTime.Parse($"{date} {time}");
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

    }
}
