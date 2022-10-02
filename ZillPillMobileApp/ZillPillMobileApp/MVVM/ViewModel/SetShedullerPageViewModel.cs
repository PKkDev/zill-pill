using System.Collections.ObjectModel;
using ZillPillMobileApp.Core;
using ZillPillMobileApp.Domain;
using ZillPillMobileApp.Infrastructure.Services;
using ZillPillMobileApp.MVVM.Model;
using ZillPillMobileApp.Templates.PageState;

namespace ZillPillMobileApp.MVVM.ViewModel
{
    public class ShedullerItem
    {
        public TimeSpan Time { get; set; }
        public double Quantity { get; set; }

        public ShedullerItem()
        {
            Time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
            Quantity = 1.0;
        }
    }

    public class ShedullerDay : ObservableObject
    {
        public DayOfWeek DayOfWeek { get; set; }
        public string DayOfWeekStr { get; set; }

        private bool _isEnambled;
        public bool IsEnambled
        {
            get { return _isEnambled; }
            set { OnSetNewValue(ref _isEnambled, value); }
        }

        public ShedullerDay(DayOfWeek dayOfWeek)
        {
            DayOfWeek = dayOfWeek;

            var cultureLang = "ru-RU"; // "en-US" "ru-RU"
            var culture = new System.Globalization.CultureInfo(cultureLang);
            string[] names = culture.DateTimeFormat.AbbreviatedDayNames;
            DayOfWeekStr = names[(int)dayOfWeek];
        }
    }

    public class SetShedullerPageViewModel : BaseViewModel
    {
        private MedicalProductDataService _mpService => DependencyService.Get<MedicalProductDataService>();

        public RelyCommand BackCommand { get; set; }

        public RelyCommand AddShedullerCommand { get; set; }

        public RelyCommand ActiveDayCommand { get; set; }

        public RelyCommand DoneCommand { get; set; }

        public RelyCommand DeleteShedItemCommand { get; set; }

        private string _selectedType;
        public string SelectedType
        {
            get { return _selectedType; }
            set
            {
                if (value == "Every day")
                    VisualStateManager.GoToState(_pageStack, "ForEveryDay");
                if (value == "By day of week")
                    VisualStateManager.GoToState(_pageStack, "ForByDayOfWeek");
                OnSetNewValue(ref _selectedType, value);
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set { OnSetNewValue(ref _startDate, value); CalckTotalDayes(); }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set { OnSetNewValue(ref _endDate, value); CalckTotalDayes(); }
        }

        private int _totalDayes;
        public int TotalDayes
        {
            get { return _totalDayes; }
            set { OnSetNewValue(ref _totalDayes, value); }
        }

        public ObservableCollection<ShedullerItem> Shedullers { get; set; }
        public ObservableCollection<ShedullerDay> ShedullerDayes { get; set; }

        private int _relationId;
        private StateContainer _pageStack;

        private SetShedullerToUserDto _shedullerData;
        public SetShedullerToUserDto ShedullerData
        {
            get { return _shedullerData; }
            set { OnSetNewValue(ref _shedullerData, value); }
        }

        public SetShedullerPageViewModel(int relationId, StateContainer pageStack)
        {
            _relationId = relationId;
            _pageStack = pageStack;

            Shedullers = new();

            ShedullerDayes = new() {
                new ShedullerDay(DayOfWeek.Monday), new ShedullerDay(DayOfWeek.Tuesday),
                new ShedullerDay(DayOfWeek.Wednesday), new ShedullerDay(DayOfWeek.Thursday),
                new ShedullerDay(DayOfWeek.Friday), new ShedullerDay(DayOfWeek.Saturday),
                new ShedullerDay(DayOfWeek.Sunday) };

            Task.Run(async () =>
            {
                try
                {
                    var data = await _mpService.GetMedProdShedullerForUserAsync(_relationId);

                    ShedullerData = data;

                    StartDate = ShedullerData.DateStart == null ? DateTime.Today : (DateTime)ShedullerData.DateStart;
                    EndDate = ShedullerData.DateEnd == null ? DateTime.Today.AddDays(10) : (DateTime)ShedullerData.DateEnd;
                    CalckTotalDayes();

                    SelectedType = "Every day";
                    switch (ShedullerData.ShedullerType)
                    {
                        case ShedullerType.EveryDay: SelectedType = "Every day"; break;
                        case ShedullerType.DayOfWeek: SelectedType = "By day of week"; break;
                    }

                    ShedullerData.DayOfWeeks.ForEach(day =>
                    {
                        ShedullerDayes.FirstOrDefault(x => x.DayOfWeek == day).IsEnambled = true;
                    });

                    ShedullerData.ShedullerItems.ForEach(item =>
                    {
                        Shedullers.Add(new ShedullerItem()
                        {
                            Time = item.Time,
                            Quantity = item.Quantity
                        });
                    });

                    PageState = PageStates.Normal;
                }
                catch (Exception e)
                {
                    MessagingCenter.Send<ErrorMessage>(new ErrorMessage(e.Message), "Error");
                }
            });

            BackCommand = new(async (param) => await Shell.Current.GoToAsync(".."));

            ActiveDayCommand = new((param) =>
            {
                var medDpod = param as ShedullerDay;
                medDpod.IsEnambled = !medDpod.IsEnambled;
            });

            AddShedullerCommand = new((param) =>
            {
                if (Shedullers.Count <= 5)
                    Shedullers.Add(new ShedullerItem());
            });

            DeleteShedItemCommand = new((param) =>
            {
                var item = param as ShedullerItem;
                var found = Shedullers.FirstOrDefault(x => x.Time == item.Time && x.Quantity == item.Quantity);
                if (found != null)
                    Shedullers.Remove(found);
            });

            DoneCommand = new(async (param) =>
            {
                try
                {
                    ShedullerType? type = null;
                    switch (SelectedType)
                    {
                        case "Every day": type = ShedullerType.EveryDay; break;
                        case "By day of week": type = ShedullerType.DayOfWeek; break;
                    }

                    List<ShedullerItemDto> result = new();
                    var startMerged = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day);
                    var tempoDate = startMerged;
                    while (tempoDate <= EndDate)
                    {
                        if (type == ShedullerType.EveryDay
                        || type == ShedullerType.DayOfWeek && ShedullerDayes.Any(x => x.IsEnambled && x.DayOfWeek.Equals(tempoDate.DayOfWeek)))
                            foreach (var time in Shedullers)
                                result.Add(new ShedullerItemDto(tempoDate, time.Time, time.Quantity));

                        tempoDate = tempoDate.AddDays(1);
                    }

                    SetShedullerToUserDto query = new()
                    {
                        ProductId = ShedullerData.ProductId,
                        RelationId = ShedullerData.RelationId,
                        ShedullerType = type,
                        DayOfWeeks = ShedullerDayes.Select(x => x.DayOfWeek).ToList(),
                        DateStart = startMerged,
                        DateEnd = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day),
                        ShedullerItems = result

                    };
                    await _mpService.SetMedProdShedullerForUserAsync(query);
                    BackCommand.Execute(null);
                }
                catch (Exception e)
                {
                    MessagingCenter.Send<ErrorMessage>(new ErrorMessage(e.Message), "Error");
                }
            });
        }

        private void CalckTotalDayes() =>
            TotalDayes = (int)(EndDate - StartDate).TotalDays + 1;
    }
}
