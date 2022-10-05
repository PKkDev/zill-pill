using CommunityToolkit.Maui.Views;

namespace ZillPillMobileApp.MVVM.Popups;

public partial class TimePickerPopup : Popup
{
    private TimePickerPopupViewModel _viewModel;

    public TimePickerPopup(TimeSpan time)
    {
        InitializeComponent();

        BindingContext = _viewModel = new TimePickerPopupViewModel(time, HoursList, MinutesList);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close(_viewModel.GetSelectedTime());
    }
}