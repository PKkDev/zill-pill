using ZillPillMobileApp.MVVM.ViewModel;

namespace ZillPillMobileApp.MVVM.View.AboutPages;

public partial class HelpPage : ContentPage
{
    private HelpPageViewModel _viewModel;

    public HelpPage()
    {
        InitializeComponent();

        BindingContext = _viewModel = new HelpPageViewModel();
    }
}