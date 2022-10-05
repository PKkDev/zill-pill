using CommunityToolkit.Maui.Views;
using ZillPillMobileApp.MVVM.ViewModel;

namespace ZillPillMobileApp.MVVM.View.ShedullerPages;

public partial class SetShedullerPage : ContentPage
{
    private SetShedullerPageViewModel _viewModel;

    public SetShedullerPage(int relationId)
    {
        InitializeComponent();

        var showPopupAsync = (Popup popup) => this.ShowPopupAsync(popup);
        BindingContext = _viewModel = new SetShedullerPageViewModel(relationId, pageStateContainer, showPopupAsync);
    }
}