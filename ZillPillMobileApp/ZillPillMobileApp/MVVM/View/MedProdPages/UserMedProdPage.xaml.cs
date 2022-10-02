using ZillPillMobileApp.MVVM.ViewModel;

namespace ZillPillMobileApp.MVVM.View.MedProdPages;

public partial class UserMedProdPage : ContentPage
{
    private UserMedProdPageViewModel _viewModel;

    public UserMedProdPage()
    {
        InitializeComponent();

        var navigatePage = async (Page page) => await Navigation.PushAsync(page);

        BindingContext = _viewModel = new UserMedProdPageViewModel(navigatePage);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.OnAppearing();
    }
}