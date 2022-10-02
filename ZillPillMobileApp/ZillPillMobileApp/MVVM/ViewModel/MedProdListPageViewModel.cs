using System.Collections.ObjectModel;
using ZillPillMobileApp.Core;
using ZillPillMobileApp.Infrastructure.Services;
using ZillPillMobileApp.MVVM.Model;
using ZillPillMobileApp.MVVM.View.MedProdPages;

namespace ZillPillMobileApp.MVVM.ViewModel
{
    internal class MedProdListPageViewModel : ObservableObject
    {
        private MedicalProductDataService _mpService => DependencyService.Get<MedicalProductDataService>();

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => OnSetNewValue(ref _isBusy, value);
        }

        public ObservableCollection<MedicalPoductItemModel> MedProducts { get; set; }

        public RelyCommand RefreshItemsCommand { get; set; }

        public RelyCommand BackCommand { get; set; }

        public RelyCommand LoadMoreItemsCommand { get; set; }

        public RelyCommand ViewDetailCommand { get; set; }
        public RelyCommand AddToUserCommand { get; set; }

        private readonly int limit = 5;
        private int offset = 0;

        public RelyCommand PerformSearch { get; set; }


        private Func<Page, Task> _navigatePage;

        public MedProdListPageViewModel(Func<Page, Task> navigatePage)
        {
            MedProducts = new();

            _navigatePage = navigatePage;

            BackCommand = new(async (param) => await Shell.Current.GoToAsync(".."));

            LoadMoreItemsCommand = new(async (param) => await OnLoadMoreItemsCommand());
            RefreshItemsCommand = new(async (param) => await OnRefreshItemsCommand());

            ViewDetailCommand = new(async (param) =>
            {
                var medProd = param as MedicalPoductItemModel;
                await _navigatePage(new MedProdDetailPage(medProd.ProductId));
            });

            AddToUserCommand = new(async (param) =>
            {
                try
                {
                    var medProd = param as MedicalPoductItemModel;
                    await _mpService.AddMedProdToUser(medProd.ProductId);
                    await Shell.Current.GoToAsync("//UserMedProdListPage");
                }
                catch (Exception e)
                {
                    MessagingCenter.Send<ErrorMessage>(new ErrorMessage(e.Message), "Error");
                }
            });

            PerformSearch = new((param) =>
            {
                var searchStr = Convert.ToString(param);
            });

            Task.Run(async () =>
            {
                await Task.Delay((int)TimeSpan.FromSeconds(1).TotalMilliseconds);
                IsBusy = true;
            });
        }

        public async Task OnRefreshItemsCommand()
        {
            try
            {
                MedProducts.Clear();
                var items = await _mpService.GetMedicalPoductListAsync(offset, limit);
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

        public async Task LoadMoreProdMed()
        {
            try
            {
                MedProducts.Clear();
                var items = await _mpService.GetMedicalPoductListAsync(0, 5);
                foreach (var item in items)
                    MedProducts.Add(item);
            }
            catch (Exception e)
            {
                MessagingCenter.Send<ErrorMessage>(new ErrorMessage(e.Message), "Error");
            }
        }

        private async Task OnLoadMoreItemsCommand()
        {
            offset += 5;
            try
            {
                var items = await _mpService.GetMedicalPoductListAsync(offset, limit);
                foreach (var item in items)
                    MedProducts.Add(item);
            }
            catch (Exception e)
            {
                MessagingCenter.Send<ErrorMessage>(new ErrorMessage(e.Message), "Error");
            }
        }
    }
}
