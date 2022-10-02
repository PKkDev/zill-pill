using ZillPillMobileApp.Core;
using ZillPillMobileApp.Infrastructure.Services;
using ZillPillMobileApp.MVVM.Model;
using ZillPillMobileApp.Templates.PageState;

namespace ZillPillMobileApp.MVVM.ViewModel
{
    internal class MedProdDetailPageViewModel : BaseViewModel
    {
        private MedicalProductDataService _mpService => DependencyService.Get<MedicalProductDataService>();

        private int _productId { get; set; }

        public RelyCommand BackCommand { get; set; }

        private MedicalPoductDetailModel _productDetail;
        public MedicalPoductDetailModel ProductDetail
        {
            get { return _productDetail; }
            set { OnSetNewValue(ref _productDetail, value); }
        }

        public RelyCommand OpenWebVita { get; set; }
        public RelyCommand OpenWebYandex { get; set; }

        public MedProdDetailPageViewModel(int productId)
        {
            _productId = productId;

            BackCommand = new(async (param) => await Shell.Current.GoToAsync(".."));

            OpenWebVita = new RelyCommand(async (param)
                => await Browser.OpenAsync($"https://vitaexpress.ru/search/{ProductDetail.Name}/"));
            OpenWebYandex = new RelyCommand(async (param)
                => await Browser.OpenAsync($"https://market.yandex.ru/search?text={ProductDetail.Name}"));

            Task.Run(async () => await LoadDetailAsync());
        }

        private async Task LoadDetailAsync()
        {
            try
            {
                var detail = await _mpService.GetMedicalPoductDetailAsync(_productId);
                ProductDetail = detail;
                PageState = PageStates.Normal;
            }
            catch (Exception e)
            {
                MessagingCenter.Send<ErrorMessage>(new ErrorMessage(e.Message), "Error");
            }
        }
    }
}
