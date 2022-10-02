using ZillPillMobileApp.Templates.PageState;

namespace ZillPillMobileApp.Core
{
    public abstract class BaseViewModel : ObservableObject
    {
        private PageStates _pageState = PageStates.Loading;
        public PageStates PageState
        {
            get => _pageState;
            set => OnSetNewValue(ref _pageState, value);
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => OnSetNewValue(ref _title, value);
        }
    }
}
