using ZillPillMobileApp.Core;
using ZillPillMobileApp.Domain.DTO.User;
using ZillPillMobileApp.Infrastructure.Services;
using ZillPillMobileApp.MVVM.Model;

namespace ZillPillMobileApp.MVVM.ViewModel
{
    public class UserPageViewModel : ObservableObject
    {
        private UserDataService _userService => DependencyService.Get<UserDataService>();

        public RelyCommand RefreshCommand { get; set; }
        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => OnSetNewValue(ref _isRefreshing, value);
        }

        private UserDetailDto _userDetail;
        public UserDetailDto UserDetail
        {
            get => _userDetail;
            set => OnSetNewValue(ref _userDetail, value);
        }

        public UserPageViewModel()
        {
            Task.Run(async () => await GetUserData());

            RefreshCommand = new RelyCommand(async (param) => await GetUserData());
        }

        private async Task GetUserData()
        {
            try
            {
                var detail = await _userService.GetUserDetailAsync();
                UserDetail = detail;
                IsRefreshing = false;
            }
            catch (Exception e)
            {
                MessagingCenter.Send<ErrorMessage>(new ErrorMessage(e.Message), "Error");
            }
        }
    }
}
