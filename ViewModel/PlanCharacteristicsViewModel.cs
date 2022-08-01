namespace Shaw.Srp.ViewModel
{
    public class PlanCharacteristicsViewModel : ViewModelBase
    {
        public PlanCharacteristicsViewModel(
            string planName,
            float monthlyServiceCharge,
            int onPeakStartHour,
            int onPeakEndHour,
            float summerOnPeak,
            float summerOffPeak,
            float summerPeakOnPeak,
            float summerPeakOffPeak,
            float winterOnPeak,
            float winterOffPeak)
        {
            PlanName = planName;
            MonthlyServiceCharge = monthlyServiceCharge;
            OnPeakStartHour = onPeakStartHour;
            OnPeakEndHour = onPeakEndHour;
            SummerOnPeak = summerOnPeak;
            SummerOffPeak = summerOffPeak;
            SummerPeakOnPeak = summerPeakOnPeak;
            SummerPeakOffPeak = summerPeakOffPeak;
            WinterOnPeak = winterOnPeak;
            WinterOffPeak = winterOffPeak;
        }

        public string PlanName { get; }

        public float MonthlyServiceCharge { get; }

        public int OnPeakStartHour { get; }

        public int OnPeakEndHour { get; }

        public float SummerOnPeak { get; }

        public float SummerOffPeak { get; }

        public float SummerPeakOnPeak { get; }

        public float SummerPeakOffPeak { get; }

        public float WinterOnPeak { get; }

        public float WinterOffPeak { get; }

        public float TotalCost
        {
            get => _TotalCost;
            set
            {
                _TotalCost = value;
                RaisePropertyChanged();
            }
        } private float _TotalCost;
    }
}
