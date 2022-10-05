using Newtonsoft.Json;

namespace ZillPillMobileApp.Domain
{
    public class SetShedullerToUserDto
    {
        [JsonProperty("productId")]
        public int ProductId { get; set; }

        [JsonProperty("relationId")]
        public int RelationId { get; set; }

        [JsonProperty("shedullerType")]
        public ShedullerType? ShedullerType { get; set; }

        [JsonProperty("dayOfWeeks")]
        public List<DayOfWeek> DayOfWeeks { get; set; }

        [JsonProperty("dateStart")]
        public DateTime? DateStart { get; set; }

        [JsonProperty("dateEnd")]
        public DateTime? DateEnd { get; set; }

        [JsonProperty("shedullerItems")]
        public List<ShedullerItemDto> ShedullerItems { get; set; }

        public SetShedullerToUserDto()
        {
            DayOfWeeks = new();
            ShedullerItems = new();
        }
    }

    public class ShedullerItemDto
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("time")]
        public TimeSpan Time { get; set; }

        //[JsonProperty("compaitDateTime")]
        //public DateTime CompaitDateTime { get; set; }

        [JsonProperty("quantity")]
        public double Quantity { get; set; }

        public ShedullerItemDto(DateTime date, TimeSpan time, double quantity)//, DateTime compaitDateTime)
        {
            Date = date;
            Time = time;
            Quantity = quantity;

            // CompaitDateTime = compaitDateTime;
        }
    }

    public enum ShedullerType
    {
        EveryDay,
        DayOfWeek
    }
}
