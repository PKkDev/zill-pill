using ZillPillMobileApp.MVVM.ViewModel;

namespace ZillPillMobileApp.MVVM.View.MedProdPages;

public partial class MedProdListPage : ContentPage
{
    private MedProdListPageViewModel _viewModel;

    public MedProdListPage()
    {
        InitializeComponent();

        var navigatePage = async (Page page) => await Navigation.PushAsync(page);

        BindingContext = _viewModel = new MedProdListPageViewModel(navigatePage);
    }
}