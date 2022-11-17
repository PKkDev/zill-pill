using System.Collections.ObjectModel;
using ZillPillMobileApp.Core;

namespace ZillPillMobileApp.MVVM.ViewModel
{
    public class UserTutorialPageViewModel : ObservableObject
    {
        public RelyCommand AcceptCommand { get; set; }
        public RelyCommand BackCommand { get; set; }

        public UserTutorialPageViewModel()
        {
            AcceptCommand = new RelyCommand(async (param) => await Shell.Current.GoToAsync("//Calendar"));
            BackCommand = new RelyCommand(async (param) => await Shell.Current.GoToAsync("//LogInPage"));
        }
    }
}
