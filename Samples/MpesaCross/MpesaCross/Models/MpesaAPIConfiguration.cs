namespace MpesaCross.Models
{
    // Note: It is better to use a web or cloud server to store all this credentials in a key vault.
    public class MpesaAPIConfiguration
    {
        public string ConsumerKey { get; set; } = "N2PCnO12dV9eZMJXafBPOX8mGYDY5dbq";
        public string ConsumerSecret { get; set; } = "gZhFpwuON2MxiQ6Z";
        public string PassKey { get; set; } = "bfb279f9aa9bdbcf158e97dd71a467cd2e0c893059b10f78e6b72ada1ed2c919";
        public string ConfirmationUrl { get; set; } = "{Your test/production confirmationurl for receiving C2B payments}";
        public string ValidationUrl { get; set; } = "{Your test/production validationurl for receiving C2B payments}";
        public string CallBackUrl { get; set; } = "https://mpesasdk-web.conveyor.cloud/api/PaymentCallback/{requestId}/StkPushCallback";
        public string LNMOshortCode { get; set; } = "174379";
        public string C2BShortCode { get; set; } = "{Your test/production C2BShortCode}";
        public string C2BMSISDNTest { get; set; } = "{Your test/production C2BMSISDN}";
    }
}
