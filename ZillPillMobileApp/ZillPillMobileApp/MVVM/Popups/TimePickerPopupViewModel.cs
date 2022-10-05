using System.Collections.ObjectModel;
using ZillPillMobileApp.Core;

namespace ZillPillMobileApp.MVVM.Popups
{
    public class TimePickerModel
    {
        public string ViewValue { get; set; }
        public int Value { get; set; }

        public TimePickerModel(int value)
        {
            Value = value;
            ViewValue = value < 10 ? $"0{value}" : value.ToString();
        }
    }

    public class TimePickerPopupViewModel : ObservableObject
    {
        public ObservableCollection<TimePickerModel> HoursList { get; set; }
        private TimePickerModel _selectedHour;
        public TimePickerModel SelectedHour
        {
            get { return _selectedHour; }
            set { OnSetNewValue(ref _selectedHour, value); }
        }
        public RelyCommand SelectedHourChangeCommand { get; set; }

        public ObservableCollection<TimePickerModel> MinutesList { get; set; }
        private TimePickerModel _selectedMinute;
        public TimePickerModel SelectedMinute
        {
            get { return _selectedMinute; }
            set { OnSetNewValue(ref _selectedMinute, value); }
        }
        public RelyCommand SelectedMinuteChangeCommand { get; set; }

        public CollectionView _hoursList { get; set; }
        public CollectionView _minutesList { get; set; }

        public RelyCommand AcceptCommand { get; set; }

        public TimePickerPopupViewModel(TimeSpan time, CollectionView hoursList, CollectionView minutesList)
        {
            _hoursList = hoursList;
            _minutesList = minutesList;

            HoursList = new();
            for (int i = 0; i <= 23; i++) HoursList.Add(new(i));

            MinutesList = new();
            for (int i = 0; i <= 59; i++) MinutesList.Add(new(i));

            SelectedHourChangeCommand = new RelyCommand((param) =>
            {
                var selected = HoursList.FirstOrDefault(x => x.Value == SelectedHour.Value);
                _hoursList.ScrollTo(selected, position: ScrollToPosition.Center, animate: false);
            });

            SelectedMinuteChangeCommand = new RelyCommand((param) =>
            {
                var selected = MinutesList.FirstOrDefault(x => x.Value == SelectedMinute.Value);
                _minutesList.ScrollTo(selected, position: ScrollToPosition.Center, animate: false);
            });

            Task.Run(async () =>
            {
                await Task.Delay((int)TimeSpan.FromSeconds(1).TotalMilliseconds);

                var nowHours = HoursList.FirstOrDefault(x => x.Value == time.Hours);
                SelectedHour = nowHours;

                var nowMin = MinutesList.FirstOrDefault(x => x.Value == time.Minutes);
                SelectedMinute = nowMin;
            });
        }

        public TimeSpan GetSelectedTime()
        {
            return new TimeSpan(SelectedHour.Value, SelectedMinute.Value, 0);
        }
    }
}
