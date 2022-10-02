namespace ZillPillMobileApp.MVVM.Model
{
    public class ShedullerItemDetailModel
    {
        public int ShedullerItemId { get; set; }

        public DateTime Date { get; set; }

        public string Time { get; set; }

        public double Quantity { get; set; }

        public bool IsSended { get; set; }

        public bool IsAccepted { get; set; }

        public string ProductName { get; set; }

        public ImageSource ImageSource { get; set; }
    }
}
