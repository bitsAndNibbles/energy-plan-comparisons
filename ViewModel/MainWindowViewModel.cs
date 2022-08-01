using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Shaw.Srp.Model;
using Shaw.Srp.Properties;
using Shaw.Srp.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Shaw.Srp.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(List<PlanViewModel> plans)
        {
            Plans = plans;

            var savedPath = Settings.Default.MruFilePath;
            if (!string.IsNullOrEmpty(savedPath))
            {
                DataFilePath = savedPath;
                _ = LoadMruFile();
            }
        }

        private async Task LoadMruFile()
        {
            if (!string.IsNullOrEmpty(DataFilePath))
            {
                try
                {
                    await LoadFileAsync(DataFilePath);
                }
                catch (Exception)
                {
                    DataFilePath = null;
                }
            }
        }

        public string DataFilePath
        {
            get => _DataFilePath;
            private set
            {
                _DataFilePath = value;
                RaisePropertyChanged();
            }
        } private string _DataFilePath;

        public List<HourlyEntry> HourlyEntries
        {
            get => _HourlyEntries;
            set
            {
                _HourlyEntries = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(EntriesStart));
                RaisePropertyChanged(nameof(EntriesEnd));
            }
        } private List<HourlyEntry> _HourlyEntries;

        public DateTime? EntriesStart => HourlyEntries?.FirstOrDefault()?.DateTime;

        public DateTime? EntriesEnd => HourlyEntries?.LastOrDefault()?.DateTime;

        public List<PlanViewModel> Plans { get; }

        public RelayCommand LoadFileCommand
        {
            get
            {
                if (_LoadFileCommand == null)
                {
                    _LoadFileCommand = new RelayCommand(async () =>
                    {
                        var filePath = BrowseForDataFile();
                        await LoadFileAsync(filePath);
                    });
                }

                return _LoadFileCommand;
            }
        } private RelayCommand _LoadFileCommand;

        private async Task LoadFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            using (var stream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 4096,
                useAsync: true))
            {
                using (var udr = new UsageDataReader(stream))
                {
                    HourlyEntries = await udr.ReadHourlyAsync();
                }
            }

            DataFilePath = filePath;
            Settings.Default.MruFilePath = filePath;
            Settings.Default.Save();

            CalculatePrices();
        }

        private static string BrowseForDataFile()
        {
            var ofd = new OpenFileDialog()
            {
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "*.csv",
                DereferenceLinks = true,
                Filter = "Comma Separated Values|*.csv|All files|*.*",
                Multiselect = false,
                Title = "Open hourly energy data file"
            };

            ofd.ShowDialog(Application.Current.MainWindow);

            return ofd.FileName;
        }

        private void CalculatePrices()
        {
            foreach (var plan in Plans)
            {
                CalculatePrice(plan);
            }
        }

        private void CalculatePrice(PlanViewModel plan)
        {
            float monthlyKWhAccumulated = 0;
            int month = -1;
            int numberOfMonths = 0;
            float maxOnPeakHourKW = 0;
            float demandCharge;

            float totalCost = plan.MonthlyServiceFee * NumberOfMonths(HourlyEntries);
            foreach (var record in HourlyEntries)
            {
                if (record.DateTime.Month != month)
                {
                    if (month > 0)
                    {
                        demandCharge = plan.DemandCharge(month, maxOnPeakHourKW);
                        totalCost += demandCharge;
                        if (demandCharge > 0)
                        {
                            Console.WriteLine($"Month {month} demand charge: ${demandCharge}, max on peak kWh: {maxOnPeakHourKW}");
                        }
                    }

                    monthlyKWhAccumulated = 0;
                    maxOnPeakHourKW = 0;
                    month = record.DateTime.Month;
                    ++numberOfMonths;
                }
                monthlyKWhAccumulated += record.KWh;
                totalCost += plan.Cost(record.DateTime, record.KWh, monthlyKWhAccumulated);
                if (plan.IsOnPeak(record.DateTime))
                {
                    maxOnPeakHourKW = Math.Max(maxOnPeakHourKW, record.KWh);
                }
            }
            demandCharge = plan.DemandCharge(month, maxOnPeakHourKW);
            totalCost += demandCharge;
            if (demandCharge > 0)
            {
                Console.WriteLine($"Month {month} demand charge: ${demandCharge}");
                ++numberOfMonths;
            }

            plan.TotalCost = totalCost;
            plan.NumberOfMonths = numberOfMonths;
        }

        private int NumberOfMonths(List<HourlyEntry> entries)
        {
            int numberOfMonths = 0;

            int priorMonth = 0;
            foreach (var entry in entries)
            {
                if (entry.DateTime.Month != priorMonth)
                {
                    priorMonth = entry.DateTime.Month;
                    ++numberOfMonths;
                }
            }

            return numberOfMonths;
        }

    }
}
