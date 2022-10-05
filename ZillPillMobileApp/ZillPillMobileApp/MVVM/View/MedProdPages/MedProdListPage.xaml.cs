using CommunityToolkit.Maui.Views;
using ZillPillMobileApp.MVVM.ViewModel;

namespace ZillPillMobileApp.MVVM.View.MedProdPages;

public partial class MedProdListPage : ContentPage
{
    private MedProdListPageViewModel _viewModel;

    public MedProdListPage()
    {
        InitializeComponent();

        var navigatePage = async (Page page) => await Navigation.PushAsync(page);
        var showPopupAsync = (Popup popup) => this.ShowPopupAsync(popup);

        BindingContext = _viewModel = new MedProdListPageViewModel(navigatePage, showPopupAsync, PageStackLayout);
    }
}