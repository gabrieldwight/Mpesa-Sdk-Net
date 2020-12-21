using Acr.UserDialogs;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Drawing;
using Xamarin.Essentials;

namespace MpesaCross.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject, IInitialize, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
        }

        ~ViewModelBase()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        public bool IsNotConnected { get; set; }
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _title, value);
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _isBusy, value);
            }
        }

        public bool IsNotBusy
        {
            get
            {
                return !IsBusy;
            }
        }

        protected void BindBusy(IReactiveCommand command)
        {
            command.IsExecuting.Subscribe(
                x => this.IsBusy = x,
                _ => this.IsBusy = false,
                () => this.IsBusy = false);
        }

        // INavigationAware
        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        // INavigationAware
        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        // IDestructible
        public virtual void Destroy()
        {

        }

        public void Initialize(INavigationParameters parameters)
        {

        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                UserDialogs.Instance.Toast(new ToastConfig("Oops, looks like you don't have internet connection")
                    .SetBackgroundColor(Color.Red)
                    .SetPosition(ToastPosition.Bottom));
            }
            else
            {
                UserDialogs.Instance.Toast(new ToastConfig("Your internet connection is back")
                    .SetBackgroundColor(Color.Green)
                    .SetPosition(ToastPosition.Bottom));
            }
        }
    }
}
