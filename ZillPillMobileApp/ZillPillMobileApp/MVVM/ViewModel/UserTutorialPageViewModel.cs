using System.Collections.ObjectModel;
using ZillPillMobileApp.Core;

namespace ZillPillMobileApp.MVVM.ViewModel
{
    public class TutorialItem
    {
        public ImageSource Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class UserTutorialPageViewModel : ObservableObject
    {
        public RelyCommand AcceptCommand { get; set; }
        public RelyCommand BackCommand { get; set; }

        public ObservableCollection<TutorialItem> TutorialItems { get; set; }

        public UserTutorialPageViewModel()
        {
            AcceptCommand = new RelyCommand(async (param) =>
            {
                await Shell.Current.GoToAsync("//Calendar");
            });

            BackCommand = new RelyCommand(async (param) =>
            {
                await Shell.Current.GoToAsync("//LogInPage");
            });

            TutorialItems = new ObservableCollection<TutorialItem>();

            TutorialItems.Add(new TutorialItem()
            {
                Image = ImageSource.FromFile("sheduller.png"),
                Title = "sheduller",
                Description = "Description1"
            });

            TutorialItems.Add(new TutorialItem()
            {
                Image = ImageSource.FromFile("sheduller.png"),
                Title = "sheduller2",
                Description = "Description2"
            });

            TutorialItems.Add(new TutorialItem()
            {
                Image = ImageSource.FromFile("sheduller.png"),
                Title = "sheduller3",
                Description = "Description3"
            });
        }
    }
}
