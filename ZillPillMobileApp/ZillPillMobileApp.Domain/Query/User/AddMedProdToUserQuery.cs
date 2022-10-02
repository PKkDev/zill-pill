namespace ZillPillMobileApp.Domain.Query.User
{
    public class AddMedProdToUserQuery
    {
        public int ProductId { get; set; }

        public List<AddMedProdToUserShedullerQuery> Shedullers { get; set; }

        public AddMedProdToUserQuery()
        {
            Shedullers = new();
        }
    }

    public class AddMedProdToUserShedullerQuery
    {
        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public AddMedProdToUserShedullerQuery() { }

        public AddMedProdToUserShedullerQuery(DateTime date, TimeSpan time)
        {
            Date = date;
            Time = time;
        }
    }
}
