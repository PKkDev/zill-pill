namespace ZillPillService.Domain.Query.User
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
        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan? Time { get; set; }
    }
}
