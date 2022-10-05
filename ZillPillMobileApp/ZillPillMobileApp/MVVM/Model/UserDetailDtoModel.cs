using ZillPillMobileApp.Core;

namespace ZillPillMobileApp.MVVM.Model
{
    public class UserDetailDtoModel : ObservableObject
    {
        public string Phone { get; set; }

        private string _email;
        public string Email
        {
            get => _email;
            set => OnSetNewValue(ref _email, value);
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set => OnSetNewValue(ref _firstName, value);
        }

        public UserDetailDtoModel(string phone, string email, string firstName)
        {
            Phone = phone;
            Email = email;
            FirstName = firstName;
        }

        public bool IsValide()
        {
            return !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(Email);
        }
    }
}
