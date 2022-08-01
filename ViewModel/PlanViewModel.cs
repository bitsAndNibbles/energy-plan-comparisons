using Shaw.Srp.Model;
using System;

namespace Shaw.Srp.ViewModel
{
    public class PlanViewModel : ViewModelBase
    {
        private Plan _Plan;

        public PlanViewModel(Plan plan)
        {
            _Plan = plan;
        }

        public string PlanName => _Plan.Name;

        public float MonthlyServiceFee => _Plan.MonthlyServiceFee;

        public float TotalCost
        {
            get => _TotalCost;
            set
            {
                _TotalCost = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(AnnualTotalCost));
            }
        } private float _TotalCost;

        public float Cost(DateTime dateTime, float kWh, float monthlyKWhAccumulated)
        {
            return _Plan.Cost(dateTime, kWh, monthlyKWhAccumulated);
        }

        public int NumberOfMonths
        {
            get => _NumberOfMonths;
            set
            {
                _NumberOfMonths = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(AnnualTotalCost));
            }
        } private int _NumberOfMonths;

        public float AnnualTotalCost
        {
            get => _TotalCost / (float)NumberOfMonths * 12f;
        }

        public bool IsOnPeak(DateTime dateTime)
        {
            return _Plan.IsOnPeak(dateTime);
        }

        public float DemandCharge(int month, float peakKW)
        {
            return _Plan.DemandCharge(month, peakKW);
        }

    }
}
