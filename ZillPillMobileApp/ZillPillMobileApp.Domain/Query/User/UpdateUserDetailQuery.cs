namespace ZillPillMobileApp.Domain.Query.User
{
    public class UpdateUserDetailQuery
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public UpdateUserDetailQuery(string email, string firstName)
        {
            Email = email;
            FirstName = firstName;
        }
    }
}
