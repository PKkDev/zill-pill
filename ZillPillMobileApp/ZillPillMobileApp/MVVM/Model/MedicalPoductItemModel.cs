namespace ZillPillMobileApp.MVVM.Model
{
    public class MedicalPoductItemModel
    {
        public int ProductId { get; set; }

        public int RelationId { get; set; }

        public string Name { get; set; }

        public ImageSource ImageSource { get; set; }

        public int TotalToAccept { get; set; }

        public int TotalAccepted { get; set; }

        public double Progress { get; set; }
        public string ProgressStr { get; set; }

        public MedicalPoductItemModel(
            int productId, int relationId, string name, ImageSource imageSource,
            int totalToAccept, int totalAccepted, double progress)
        {
            ProductId = productId;
            RelationId = relationId;
            Name = name;
            ImageSource = imageSource;

            TotalToAccept = totalToAccept;
            TotalAccepted = totalAccepted;
            Progress = progress;
            ProgressStr = $"{totalAccepted}/{totalToAccept}";
        }

    }
}
