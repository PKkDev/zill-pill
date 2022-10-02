using ZillPillMobileApp.MVVM.ViewModel;

namespace ZillPillMobileApp.MVVM.View;

public partial class AuthorizePage : ContentPage
{
    private AuthorizePageViewModel _viewModel;

    public AuthorizePage()
    {
        InitializeComponent();

        BindingContext = _viewModel = new AuthorizePageViewModel(GridBase);
    }
}