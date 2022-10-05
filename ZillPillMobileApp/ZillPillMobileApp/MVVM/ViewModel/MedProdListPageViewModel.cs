using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using ZillPillMobileApp.Core;
using ZillPillMobileApp.Domain.Query;
using ZillPillMobileApp.Infrastructure.Services;
using ZillPillMobileApp.MVVM.Model;
using ZillPillMobileApp.MVVM.Popups;
using ZillPillMobileApp.MVVM.View.MedProdPages;

namespace ZillPillMobileApp.MVVM.ViewModel
{
    internal class MedProdListPageViewModel : ObservableObject
    {
        private MedicalProductDataService _mpService => DependencyService.Get<MedicalProductDataService>();
        private CountriesDataService _conytryService => DependencyService.Get<CountriesDataService>();

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => OnSetNewValue(ref _isBusy, value);
        }

        public ObservableCollection<MedicalPoductItemModel> MedProducts { get; set; }

        public RelyCommand RefreshItemsCommand { get; set; }

        public RelyCommand BackCommand { get; set; }

        public RelyCommand LoadMoreItemsCommand { get; set; }

        public RelyCommand ViewDetailCommand { get; set; }
        public RelyCommand AddToUserCommand { get; set; }

        private readonly int limit = 5;
        private int offset = 0;

        public RelyCommand AcceptFilterCommand { get; set; }

        public RelyCommand CleareFilterCommand { get; set; }

        public List<CountryDetailModel> Countries { get; set; } = new();
        private CountryDetailModel _selectedCountry;
        public CountryDetailModel SelectedCountry
        {
            get { return _selectedCountry; }
            set { OnSetNewValue(ref _selectedCountry, value); }
        }
        public RelyCommand OpenCountriesPopupCommand { get; set; }

        public RelyCommand SetFilterCommand { get; set; }

        private Func<Page, Task> _navigatePage;
        private Func<Popup, Task<object>> _showPopupAsync;
        VerticalStackLayout _pageStackLayout;

        private GetFilteredMedicalProductModel _filter;
        public GetFilteredMedicalProductModel Filter
        {
            get => _filter;
            set => OnSetNewValue(ref _filter, value);
        }

        private GetFilteredMedicalProductQuery filterQuery;


        private bool IsViewFIlter = false;

        public MedProdListPageViewModel(Func<Page, Task> navigatePage, Func<Popup, Task<object>> showPopupAsync, VerticalStackLayout pageStackLayout)
        {
            filterQuery = new();
            Filter = new();

            MedProducts = new();

            _navigatePage = navigatePage;
            _pageStackLayout = pageStackLayout;
            _showPopupAsync = showPopupAsync;

            VisualStateManager.GoToState(_pageStackLayout, "WithOutFilter");

            BackCommand = new(async (param) => await Shell.Current.GoToAsync(".."));

            LoadMoreItemsCommand = new(async (param) => await OnLoadMoreItemsCommand());
            RefreshItemsCommand = new(async (param) => await OnRefreshItemsCommand());

            ViewDetailCommand = new(async (param) =>
            {
                var medProd = param as MedicalPoductItemModel;
                await _navigatePage(new MedProdDetailPage(medProd.ProductId));
            });

            AddToUserCommand = new(async (param) =>
            {
                try
                {
                    var medProd = param as MedicalPoductItemModel;
                    await _mpService.AddMedProdToUser(medProd.ProductId);
                    await Shell.Current.GoToAsync("//UserMedProdListPage");
                }
                catch (Exception e)
                {
                    MessagingCenter.Send<ErrorMessage>(new ErrorMessage(e.Message), "Error");
                }
            });

            OpenCountriesPopupCommand = new(async (param) =>
            {
                var result = await _showPopupAsync(new CountrySelectorPoup(Countries));
                if (result is CountryDetailModel countryResult) SelectedCountry = countryResult;
                else SelectedCountry = null;
            });

            AcceptFilterCommand = new((param) =>
            {
                MedProducts.Clear();
                offset = 0;
                filterQuery = new GetFilteredMedicalProductQuery(
                    SelectedCountry?.CountryId ?? null,
                   String.IsNullOrEmpty(Filter.ProductName) ? null : Filter.ProductName,
                   Filter.WithCertificate,
                   String.IsNullOrEmpty(Filter.ChemicalName) ? null : Filter.ChemicalName
                    );
                IsBusy = true;
            });

            CleareFilterCommand = new((param) =>
            {
                MedProducts.Clear();
                offset = 0;
                SelectedCountry = null;
                filterQuery = new();
                Filter = new();
                IsBusy = true;
            });

            SetFilterCommand = new((param) =>
            {
                IsViewFIlter = !IsViewFIlter;
                string state = IsViewFIlter ? "WithFilter" : "WithOutFilter";
                VisualStateManager.GoToState(_pageStackLayout, state);
            });

            Task.Run(async () =>
            {
                await Task.Delay((int)TimeSpan.FromSeconds(1).TotalMilliseconds);
                IsBusy = true;

                Countries = await _conytryService.GetCountriesAsync();
            });
        }

        public async Task OnRefreshItemsCommand()
        {
            try
            {
                MedProducts.Clear();
                var items = await _mpService.GetMedicalPoductListAsync(filterQuery, offset, limit);
                foreach (var item in items)
                    MedProducts.Add(item);
            }
            catch (Exception e)
            {
                MessagingCenter.Send<ErrorMessage>(new ErrorMessage(e.Message), "Error");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnLoadMoreItemsCommand()
        {
            offset += 5;
            try
            {
                var items = await _mpService.GetMedicalPoductListAsync(filterQuery, offset, limit);
                foreach (var item in items)
                    MedProducts.Add(item);
            }
            catch (Exception e)
            {
                MessagingCenter.Send<ErrorMessage>(new ErrorMessage(e.Message), "Error");
            }
        }
    }
}
