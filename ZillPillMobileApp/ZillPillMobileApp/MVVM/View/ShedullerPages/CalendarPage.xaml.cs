using ZillPillMobileApp.MVVM.ViewModel;

namespace ZillPillMobileApp.MVVM.View.ShedullerPages;

public partial class CalendarPage : ContentPage
{
    private CalendarPageViewModel _viewModel;

    public CalendarPage()
    {
        InitializeComponent();

        BindingContext = _viewModel = new CalendarPageViewModel(DayesCollection);
    }

    private void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        // left
        if (e.HorizontalDelta < 0)
        {
            //_viewModel.AddPrevsDayes();
        }

        // right
        if (e.HorizontalDelta > 0)
        {
            _viewModel.AddNextDayes();
        }
    }
}