using CommunityToolkit.Maui.Views;
using ZillPillMobileApp.MVVM.Model;

namespace ZillPillMobileApp.MVVM.Popups;

public partial class CountrySelectorPoup : Popup
{
    private CountrySelectorPoupViewModel _viewModel;

    public CountrySelectorPoup(List<CountryDetailModel> countries)
    {
        InitializeComponent();

        BindingContext = _viewModel = new CountrySelectorPoupViewModel(countries);
    }

    private void Accept_Button_Clicked(object sender, EventArgs e)
        => Close(_viewModel.GetSelectedTime());


    private void Cleare_Button_Clicked(object sender, EventArgs e)
        => Close(null);

    private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        _viewModel.FilterCountries(searchBar.Text);
    }
}