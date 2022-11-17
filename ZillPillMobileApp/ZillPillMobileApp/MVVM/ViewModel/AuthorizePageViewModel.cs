using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using ZillPillMobileApp.Core;
using ZillPillMobileApp.Infrastructure.Services;
using ZillPillMobileApp.MVVM.Model;

#if ANDROID
using Firebase.Messaging;
#endif

namespace ZillPillMobileApp.MVVM.ViewModel
{
    public class AuthorizePageViewModel : ObservableObject
    {
        private UserDataService _userService => DependencyService.Get<UserDataService>();

        private bool _isNewUser = true;

        private string _phone;
        public string Phone
        {
            get => _phone;
            set => OnSetNewValue(ref _phone, value);
        }

        private string _code;
        public string Code
        {
            get => _code;
            set => OnSetNewValue(ref _code, value);
        }

        public RelyCommand GetCodeCommand { get; set; }
        public RelyCommand CheckCodeCommand { get; set; }
        public RelyCommand FingerLogInCommand { get; set; }

        private bool _logInPocessing = false;
        public bool LogInPocessing
        {
            get => _logInPocessing;
            set => OnSetNewValue(ref _logInPocessing, value);
        }

        private Microsoft.Maui.Controls.Grid _gridBase;

        public AuthorizePageViewModel(Microsoft.Maui.Controls.Grid gridBase)
        {
            Phone = "89372174165";
            Code = "sdasd";

            GetCodeCommand = new RelyCommand(async (param) => await GetCodeAsync());
            CheckCodeCommand = new RelyCommand(async (param) => await CheckCodeAsync());
            FingerLogInCommand = new RelyCommand(async (paam) => { await OnFingerLogInCommand(); });

            _gridBase = gridBase;
            VisualStateManager.GoToState(gridBase, "PhoneView");

            Task.Run(async () =>
            {
                await Task.Delay((int)TimeSpan.FromSeconds(2).TotalMilliseconds);
                var phone = await SecureStorage.GetAsync("phone");
                var code = await SecureStorage.GetAsync("code");
                if (!string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(code))
                {
                    MainThread.BeginInvokeOnMainThread(() => { FingerLogInCommand.Execute(null); });
                    _isNewUser = false;
                }

            });
        }

        private async Task GetCodeAsync()
        {
            LogInPocessing = true;
            try
            {
                var phone = await SecureStorage.GetAsync("phone");
                if (!string.IsNullOrEmpty(phone) && phone != Phone)
                    _isNewUser = true;

                await _userService.GetCodeForUserAsync(Phone);
                VisualStateManager.GoToState(_gridBase, "CodeView");
            }
            catch (Exception e)
            {
                SecureStorage.RemoveAll();
                MessagingCenter.Send<ErrorMessage>(new ErrorMessage(e.Message), "Error");
            }
            finally
            {
                LogInPocessing = false;
            }
        }

        private async Task CheckCodeAsync()
        {
            LogInPocessing = true;
            try
            {
                var cred = await _userService.CheckCodeForUserAsync(Phone, Code);
                await SecureStorage.SetAsync("phone", Phone);
                await SecureStorage.SetAsync("code", Code);
                await SecureStorage.SetAsync("token", cred.Token);

#if ANDROID
                FirebaseMessaging.Instance.SubscribeToTopic($"sheduller_{Phone}");
                FirebaseMessaging.Instance.SubscribeToTopic($"system");
#endif

                if (_isNewUser)
                    await Shell.Current.GoToAsync("//UserTutorial");
                else
                    await Shell.Current.GoToAsync("//Calendar");
            }
            catch (Exception e)
            {
                SecureStorage.RemoveAll();
                MessagingCenter.Send<ErrorMessage>(new ErrorMessage(e.Message), "Error");
            }
            finally
            {
                LogInPocessing = false;
            }
        }

        private async Task OnFingerLogInCommand()
        {
            var aviability = await CrossFingerprint.Current.IsAvailableAsync();

            if (!aviability)
            {
                MessagingCenter.Send<ErrorMessage>(new ErrorMessage("Biomatric is not available"), "Error");
                return;
            }

            var phone = await SecureStorage.GetAsync("phone");
            var code = await SecureStorage.GetAsync("code");
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(code))
            {
                MessagingCenter.Send<ErrorMessage>(new ErrorMessage("Needed authorize"), "Error");
                return;
            }

            var request = new AuthenticationRequestConfiguration("Auth", " I wouild like use your biometric please");
            var authResult = await CrossFingerprint.Current.AuthenticateAsync(request);

            if (authResult.Authenticated)
            {
                Phone = phone;
                Code = code;
                await CheckCodeAsync();
            }
        }
    }
}
