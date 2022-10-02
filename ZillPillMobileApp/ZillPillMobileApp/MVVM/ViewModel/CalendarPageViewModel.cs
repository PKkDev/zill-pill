using System.Collections.ObjectModel;
using ZillPillMobileApp.Core;
using ZillPillMobileApp.Infrastructure.Services;
using ZillPillMobileApp.MVVM.Model;

namespace ZillPillMobileApp.MVVM.ViewModel
{
    public class TestItem
    {
        public DateTime Date { get; set; }

        public string DayOfWeekStr { get; set; }
        public string DayStr { get; set; }

        public TestItem(DateTime date)
        {
            Date = date;

            var cultureLang = "ru-RU"; // "en-US" "ru-RU"
            var culture = new System.Globalization.CultureInfo(cultureLang);
            string[] names = culture.DateTimeFormat.AbbreviatedDayNames;
            DayOfWeekStr = names[(int)date.DayOfWeek];

            DayStr = date.ToString("dd");
        }
    }

    public class CalendarPageViewModel : BaseViewModel
    {
        private MedicalProductDataService _mpService => DependencyService.Get<MedicalProductDataService>();

        private DateTime _today;
        public DateTime Today
        {
            get { return _today; }
            set { OnSetNewValue(ref _today, value); }
        }

        private bool _shedullersIsRefreshing = false;
        public bool ShedullersIsRefreshing
        {
            get { return _shedullersIsRefreshing; }
            set { OnSetNewValue(ref _shedullersIsRefreshing, value); }
        }
        public RelyCommand RefreshShedullersCommand { get; set; }

        public ObservableCollection<TestItem> Items { get; set; }

        public ObservableCollection<ShedullerItemDetailModel> ShedItems { get; set; }

        private TestItem _selectedDay;
        public TestItem SelectedDay
        {
            get { return _selectedDay; }
            set { OnSetNewValue(ref _selectedDay, value); }
        }
        public RelyCommand SelectedDayChange { get; set; }

        public RelyCommand AcceptShedullerCommand { get; set; }

        public RelyCommand ViewNowCommand { get; set; }

        CollectionView _dayesCollection;

        public CalendarPageViewModel(CollectionView dayesCollection)
        {
            _dayesCollection = dayesCollection;
            Today = DateTime.Today;
            Items = new();
            ShedItems = new();

            var end = Today.AddDays(7);
            var start = Today.AddDays(-7);
            var tempo = start;
            while (tempo < end)
            {
                Items.Add(new TestItem(tempo));
                tempo = tempo.AddDays(1);
            }

            Task.Run(async () =>
            {
                await Task.Delay((int)TimeSpan.FromSeconds(1.5).TotalMilliseconds);
                ViewNowCommand.Execute(null);
            });

            ViewNowCommand = new((param) =>
            {
                var now = Items.FirstOrDefault(x => x.Date == Today);
                if (SelectedDay != null && SelectedDay.Date == Today)
                    _dayesCollection.ScrollTo(now, position: ScrollToPosition.Center, animate: true);
                else
                    SelectedDay = now;
            });

            SelectedDayChange = new((param) =>
            {
                var now = Items.FirstOrDefault(x => x.Date == SelectedDay.Date);
                _dayesCollection.ScrollTo(now, position: ScrollToPosition.Center, animate: true);
                ShedullersIsRefreshing = true;
            });

            RefreshShedullersCommand = new(async (param) =>
            {
                await LoadShedullersForDay();
            });

            AcceptShedullerCommand = new(async (param) =>
            {
                var item = param as ShedullerItemDetailModel;
                if (!item.IsAccepted)
                {
                    await _mpService.AcceptShedullerItemAsync(item.ShedullerItemId);
                    ShedullersIsRefreshing = true;
                }
            });
        }

        public bool prevBuisy = false;
        public void AddPrevsDayes()
        {
            if (prevBuisy)
                return;

            prevBuisy = true;

            var first = Items.First();
            for (var i = 1; i <= 3; i++)
                Items.Insert(0, new TestItem(first.Date.AddDays(-i)));

            prevBuisy = false;
        }

        public bool nextBuisy = false;
        public void AddNextDayes()
        {
            if (nextBuisy)
                return;

            nextBuisy = true;

            var last = Items.Last();
            for (var i = 1; i <= 3; i++)
                Items.Add(new TestItem(last.Date.AddDays(i)));

            nextBuisy = false;
        }

        private async Task LoadShedullersForDay()
        {
            try
            {
                ShedItems.Clear();

                var items = await _mpService.GetShedullerItemsByDayAsync(SelectedDay.Date);
                foreach (var item in items)
                    ShedItems.Add(item);
            }
            catch (Exception e)
            {
                MessagingCenter.Send(new ErrorMessage(e.Message), "Error");
            }
            finally
            {
                ShedullersIsRefreshing = false;
            }
        }
    }
}
