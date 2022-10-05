using System.Collections.ObjectModel;
using ZillPillMobileApp.Core;
using ZillPillMobileApp.MVVM.Model;

namespace ZillPillMobileApp.MVVM.Popups
{
    public class CountrySelectorPoupViewModel : ObservableObject
    {
        public ObservableCollection<CountryDetailModel> ToViewCountries { get; set; }
        public List<CountryDetailModel> TotalCountries { get; set; }

        public CountryDetailModel _selectedCountry;
        public CountryDetailModel SelectedCountry
        {
            get { return _selectedCountry; }
            set { OnSetNewValue(ref _selectedCountry, value); }
        }

        public RelyCommand CountrySearch { get; set; }

        public CountrySelectorPoupViewModel(List<CountryDetailModel> countries)
        {
            ToViewCountries = new();
            TotalCountries = new();

            TotalCountries = countries;

            foreach (var country in TotalCountries)
                ToViewCountries.Add(country);

            CountrySearch = new((param) =>
            {
                var search = Convert.ToString(param);
                this.FilterCountries(search);
            });
        }

        public void FilterCountries(string search)
        {
            var filtered = string.IsNullOrEmpty(search)
                ? TotalCountries
                : TotalCountries.Where(x => x.Name.Contains(search.ToLower()));

            ToViewCountries.Clear();
            foreach (var country in filtered)
                ToViewCountries.Add(country);
        }

        public CountryDetailModel? GetSelectedTime()
        {
            return SelectedCountry;
        }
    }
}
