using ZillPillService.Domain.Model;

namespace ZillPillService.Domain.Query.User
{
    public class SetShedullerToUserDto
    {
        public int ProductId { get; set; }

        public int RelationId { get; set; }

        public ShedullerType? ShedullerType { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public List<DayOfWeek> DayOfWeeks { get; set; }

        public List<ShedullerItem> ShedullerItems { get; set; }

        public SetShedullerToUserDto()
        {
            DayOfWeeks = new();
            ShedullerItems = new();
        }
    }

    public class ShedullerItem
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public double Quantity { get; set; }
       // public DateTime CompaitDateTime { get; set; }

        public ShedullerItem(DateTime date, TimeSpan time, double quantity)
        {
            Date = date;
            Time = time;
            Quantity = quantity;
        }
    }
}
