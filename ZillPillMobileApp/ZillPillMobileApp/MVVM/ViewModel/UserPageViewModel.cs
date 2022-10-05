using System.ComponentModel;
using ZillPillMobileApp.Core;
using ZillPillMobileApp.Domain.Query.User;
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

        private UserDetailDtoModel _userDetail;
        public UserDetailDtoModel UserDetail
        {
            get => _userDetail;
            set => OnSetNewValue(ref _userDetail, value);
        }

        public Command UpdateUserCommand { get; set; }

        public RelyCommand DeleteAccountCommand { get; set; }

        public UserPageViewModel()
        {
            Task.Run(async () => await GetUserData());

            RefreshCommand = new RelyCommand(async (param) => await GetUserData());

            UpdateUserCommand = new(
                async (param) =>
                {
                    UpdateUserDetailQuery query = new(UserDetail.Email, UserDetail.FirstName);
                    await _userService.UpdateUserDetailAsync(query);
                    IsRefreshing = true;
                },
                (param) =>
                 {
                     return UserDetail?.IsValide() ?? false;
                 });

            DeleteAccountCommand = new(
                async (param) =>
                {
                    await _userService.DeleteUserAsync();
                    await Shell.Current.GoToAsync("//LogInPage");
                });
        }

        private async Task GetUserData()
        {
            try
            {
                if (UserDetail != null)
                    UserDetail.PropertyChanged -= UserDetail_PropertyChanged;
                var detail = await _userService.GetUserDetailAsync();
                UserDetail = detail;
                UserDetail.PropertyChanged += UserDetail_PropertyChanged;
                IsRefreshing = false;
            }
            catch (Exception e)
            {
                MessagingCenter.Send<ErrorMessage>(new ErrorMessage(e.Message), "Error");
            }
        }

        private void UserDetail_PropertyChanged(object sender, PropertyChangedEventArgs e)
            => UpdateUserCommand.ChangeCanExecute();

    }
}
