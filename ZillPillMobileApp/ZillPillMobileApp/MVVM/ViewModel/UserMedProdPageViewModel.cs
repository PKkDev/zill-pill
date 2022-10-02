using System.Collections.ObjectModel;
using ZillPillMobileApp.Core;
using ZillPillMobileApp.Infrastructure.Services;
using ZillPillMobileApp.MVVM.Model;
using ZillPillMobileApp.MVVM.View.MedProdPages;
using ZillPillMobileApp.MVVM.View.ShedullerPages;

namespace ZillPillMobileApp.MVVM.ViewModel
{
    public class UserMedProdPageViewModel : ObservableObject
    {
        private MedicalProductDataService _mpService => DependencyService.Get<MedicalProductDataService>();

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => OnSetNewValue(ref _isBusy, value);
        }

        public RelyCommand RefreshItemsCommand { get; set; }
        public RelyCommand DeleteItemCommand { get; set; }
        public RelyCommand AddMedPredCommand { get; set; }
        public RelyCommand ViewDetailCommand { get; set; }
        public RelyCommand SetShedullerCommand { get; set; }

        public ObservableCollection<MedicalPoductItemModel> MedProducts { get; set; }

        Func<Page, Task> _navigatePage;

        public UserMedProdPageViewModel(Func<Page, Task> navigatePage)
        {
            _navigatePage = navigatePage;
            MedProducts = new();
            AddMedPredCommand = new(async (param) => await _navigatePage(new MedProdListPage()));
            RefreshItemsCommand = new(async (param) => await OnRefreshItemsCommand());

            ViewDetailCommand = new(async (param) =>
            {
                var medProd = param as MedicalPoductItemModel;
                await _navigatePage(new MedProdDetailPage(medProd.ProductId));
            });

            DeleteItemCommand = new(async (param) =>
            {
                try
                {
                    var medProd = param as MedicalPoductItemModel;
                    await _mpService.RemoveMedicalPoductListForUserAsync(medProd.RelationId);
                }
                catch (Exception e)
                {
                    MessagingCenter.Send<ErrorMessage>(new ErrorMessage(e.Message), "Error");
                }
                finally
                {
                    IsBusy = true;
                }
            });

            SetShedullerCommand = new(async (param) =>
            {
                var medProd = param as MedicalPoductItemModel;
                await _navigatePage(new SetShedullerPage(medProd.RelationId));
            });

            IsBusy = true;
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        private async Task OnRefreshItemsCommand()
        {
            try
            {
                MedProducts.Clear();
                var items = await _mpService.GetMedicalPoductListForUserAsync();
                foreach (var item in items)
                    MedProducts.Add(item);
            }
            catch (Exception e)
            {
                MessagingCenter.Send<ErrorMessage>(new ErrorMessage(e.Message), "Error");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
