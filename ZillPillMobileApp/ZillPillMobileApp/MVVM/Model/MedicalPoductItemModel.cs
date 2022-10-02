namespace ZillPillMobileApp.MVVM.Model
{
    public class MedicalPoductItemModel
    {
        public int ProductId { get; set; }

        public int RelationId { get; set; }

        public string Name { get; set; }

        public ImageSource ImageSource { get; set; }

        public MedicalPoductItemModel(int productId, int relationId, string name, ImageSource imageSource)
        {
            ProductId = productId;
            RelationId = relationId;
            Name = name;
            ImageSource = imageSource;
        }
    }
}
