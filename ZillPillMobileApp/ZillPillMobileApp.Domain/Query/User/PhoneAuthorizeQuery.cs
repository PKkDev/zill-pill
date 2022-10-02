namespace ZillPillMobileApp.Domain.Query.User
{
    public class PhoneAuthorizeQuery
    {
        public string Phone { get; set; }

        public PhoneAuthorizeQuery(string phone)
        {
            Phone = phone;
        }
    }
}
