namespace ZillPillService.Domain.DTO.Shedullers
{
    public class ShedullerItemDetailDto
    {
        public int ShedullerItemId { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public double Quantity { get; set; }

        public bool IsSended { get; set; }

        public bool IsAccepted { get; set; }

        public string ProductName { get; set; }

        public byte[]? ImageData { get; set; }

        public ShedullerItemDetailDto(
            int shedullerItemId, DateTime date, TimeSpan time, double quantity, bool isSended, bool isAccepted,
            string productName, byte[]? imageData)
        {
            ShedullerItemId = shedullerItemId;
            Date = date;
            Time = time;
            Quantity = quantity;
            IsSended = isSended;
            IsAccepted = isAccepted;
            ProductName = productName;
            ImageData = imageData;
        }
    }
}
