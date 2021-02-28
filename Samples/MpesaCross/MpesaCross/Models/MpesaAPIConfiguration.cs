namespace MpesaCross.Models
{
    // Note: It is better to use a web or cloud server to store all this credentials in a key vault.
    public class MpesaAPIConfiguration
    {
        public string ConsumerKey { get; set; } = "{Your test/production consumerkey}";
        public string ConsumerSecret { get; set; } = "{Your test/production consumerSecret}";
        public string PassKey { get; set; } = "{Your test/production Lipa Na Mpesa Online PassKey}";
        public string ConfirmationUrl { get; set; } = "{Your test/production confirmationurl for receiving C2B payments}";
        public string ValidationUrl { get; set; } = "{Your test/production validationurl for receiving C2B payments}";
        public string CallBackUrl { get; set; } = "{Your test/production callbackurl for receiving STK Push payments}";
        public string LNMOshortCode { get; set; } = "{Your test/production Lipa Na Mpesa Online ShortCode}";
        public string C2BShortCode { get; set; } = "{Your test/production C2BShortCode}";
        public string C2BMSISDNTest { get; set; } = "{Your test/production C2BMSISDN}";
    }
}
