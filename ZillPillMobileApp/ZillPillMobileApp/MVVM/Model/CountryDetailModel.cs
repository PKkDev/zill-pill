namespace ZillPillMobileApp.MVVM.Model
{
    public class CountryDetailModel
    {
        /// <summary>
        /// NOT USED
        /// </summary>
        public int CountryId { get; set; }

        public string Name { get; set; }

        public ImageSource ImageSource { get; set; }

        public CountryDetailModel(int countryId, string name, ImageSource imageSource)
        {
            CountryId = countryId;
            Name = name;
            ImageSource = imageSource;
        }
    }
}
