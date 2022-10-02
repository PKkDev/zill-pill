using ZillPillMobileApp.MVVM.ViewModel;

namespace ZillPillMobileApp.MVVM.View.MedProdPages;

public partial class MedProdDetailPage : ContentPage
{
    private MedProdDetailPageViewModel _viewModel;

    public MedProdDetailPage(int productId)
    {
        InitializeComponent();

        BindingContext = _viewModel = new MedProdDetailPageViewModel(productId);
    }
}