using MpesaSdk.Enums;

namespace MpesaSdk.Web.ViewModels
{
    public class CustomerToBusinessRegisterViewModel
    {
        public CustomerToBusinessResponseType customerToBusinessResponse { get; set; }
        public string ConfirmationUrl { get; set; }
        public string ValidationUrl { get; set; }
    }
}
