using Acr.UserDialogs;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace MpesaCross.ViewModels
{
    public class MpesaPushStkViewModel : ViewModelBase
    {
        #region Properties
        private string _amount;
        public string Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _amount, value);
            }
        }
        private string _accountReference;
        public string AccountReference
        {
            get
            {
                return _accountReference;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _accountReference, value);
            }
        }
        private string _transactionDescription;
        public string TransactionDescription
        {
            get
            {
                return _transactionDescription;
            }
            set 
            {
                this.RaiseAndSetIfChanged(ref _transactionDescription, value);
            }
        }
        private string _phoneNumber;
        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _phoneNumber, value);
            }
        }
        public ReactiveCommand<Unit, Unit> MpesaStkCommand { get; }
        private INavigationService _navigationService { get; }
        protected IUserDialogs _dialogs { get; }
        #endregion

        #region Methods
        public MpesaPushStkViewModel(INavigationService navigationService, IUserDialogs dialogs) : base(navigationService)
        {
            Title = "Mpesa Push Stk Payment";
            _navigationService = navigationService;
            _dialogs = dialogs;
            MpesaStkCommand = ReactiveCommand.CreateFromTask(ExecuteMpesaStkCommand,
                this.WhenAnyValue(
                    x => x.Amount,
                    x => x.AccountReference,
                    x => x.TransactionDescription,
                    x => x.PhoneNumber,
                    (amount, accountReference, transactionDescription, phoneNumber) =>
                    {
                        if (string.IsNullOrEmpty(amount) && string.IsNullOrEmpty(accountReference) && string.IsNullOrEmpty(transactionDescription) && string.IsNullOrEmpty(phoneNumber))
                        {
                            return false;
                        }
                        return true;
                    }));
            this.BindBusy(MpesaStkCommand);
        }

        private async Task ExecuteMpesaStkCommand()
        {
            try
            {

            }
            catch
            {

            }
        }
        #endregion
    }
}
