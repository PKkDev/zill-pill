namespace ZillPillMobileApp.Domain.Query.User
{
    public class CheckPhoneAuthorizeQuery : PhoneAuthorizeQuery
    {
        public string Code { get; set; }

        public CheckPhoneAuthorizeQuery(string phone, string code)
            : base(phone)
        {
            Code = code;
        }
    }
}
