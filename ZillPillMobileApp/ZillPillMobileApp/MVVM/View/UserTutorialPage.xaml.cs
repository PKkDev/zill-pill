using ZillPillMobileApp.MVVM.ViewModel;

namespace ZillPillMobileApp.MVVM.View;

public partial class UserTutorialPage : ContentPage
{
    private UserTutorialPageViewModel _viewModel;

    public UserTutorialPage()
    {
        InitializeComponent();

        BindingContext = _viewModel = new UserTutorialPageViewModel();
    }
}