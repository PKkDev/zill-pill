namespace ZillPillService.Domain.Query.User
{
    public class CheckPhoneAuthorizeQuery : PhoneAuthorizeQuery
    {
        public string Code { get; set; }
    }
}
