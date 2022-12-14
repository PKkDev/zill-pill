using System.ComponentModel;
using System.Text.RegularExpressions;
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
                    try
                    {
                        string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
                        if (!Regex.IsMatch(UserDetail.Email.Trim(), pattern, RegexOptions.IgnoreCase))
                            throw new Exception("некорректный email");

                        UpdateUserDetailQuery query = new(UserDetail.Email.Trim(), UserDetail.FirstName.Trim());
                        await _userService.UpdateUserDetailAsync(query);
                        IsRefreshing = true;
                    }
                    catch (Exception e)
                    {
                        MessagingCenter.Send<ErrorMessage>(new ErrorMessage(e.Message), "Error");
                    }
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
