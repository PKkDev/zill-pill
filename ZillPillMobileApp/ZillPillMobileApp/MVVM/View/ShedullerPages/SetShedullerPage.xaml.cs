using ZillPillMobileApp.MVVM.ViewModel;

namespace ZillPillMobileApp.MVVM.View.ShedullerPages;

public partial class SetShedullerPage : ContentPage
{
    private SetShedullerPageViewModel _viewModel;

    public SetShedullerPage(int relationId)
    {
        InitializeComponent();

        BindingContext = _viewModel = new SetShedullerPageViewModel(relationId, pageStateContainer);
    }
}