using ZillPillMobileApp.Core;

namespace ZillPillMobileApp.MVVM.ViewModel
{
    public class HelpPageViewModel : ObservableObject
    {
        public string AppName { get; set; } = AppInfo.Name;
        public string PackageName { get; set; } = AppInfo.PackageName;
        public string Version { get; set; } = AppInfo.VersionString;
        public string Build { get; set; } = AppInfo.BuildString;

        public RelyCommand OnShowSettingsUI { get; set; }

        private string _networkState;
        public string NetworkState
        {
            get { return _networkState; }
            set { OnSetNewValue(ref _networkState, value); }
        }
        public RelyCommand CheckNetwork { get; set; }

        public HelpPageViewModel()
        {
            OnShowSettingsUI = new RelyCommand((param) => AppInfo.ShowSettingsUI());

            Connectivity.ConnectivityChanged += OnConnectivityChanged;
            CheckNetwork = new RelyCommand((param) => CheckConnection());
            CheckConnection();
        }

        /// <summary>
        /// обработка изменения состояния подключения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e) => CheckConnection();

        /// <summary>
        /// проверка состояния сети
        /// </summary>
        private void CheckConnection()
        {
            NetworkState = Connectivity.NetworkAccess switch
            {
                NetworkAccess.Internet => "локальная сеть и доступ к Интернету",
                NetworkAccess.ConstrainedInternet => "ограниченный доступ к Интернету",
                NetworkAccess.Local => "только доступ к локальной сети",
                NetworkAccess.None => "подключение недоступно",
                NetworkAccess.Unknown => "не удается определить режим подключения",
                _ => "неизвестное состояние",
            };
        }
    }
}
