using Newtonsoft.Json;

namespace MpesaSdk.Callbacks
{
    /// <summary>
    /// Callback to handle C2B result response
    /// </summary>
    public class CustomerToBusinessCallback
    {
        [JsonProperty("TransactionType")]
        public string TransactionType { get; set; }
        [JsonProperty("TransID")]
        public string TransID { get; set; }
        [JsonProperty("TransTime")]
        public string TransTime { get; set; }
        [JsonProperty("TransAmount")]
        public string TransAmount { get; set; }
        [JsonProperty("BusinessShortCode")]
        public string BusinessShortCode { get; set; }
        [JsonProperty("BillRefNumber")]
        public string BillRefNumber { get; set; }
        [JsonProperty("InvoiceNumber")]
        public string InvoiceNumber { get; set; }
        [JsonProperty("OrgAccountBalance")]
        public string OrgAccountBalance { get; set; }
        [JsonProperty("ThirdPartyTransID")]
        public string ThirdPartyTransID { get; set; }
        [JsonProperty("MSISDN")]
        public string MSISDN { get; set; }
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }
        [JsonProperty("MiddleName")]
        public string MiddleName { get; set; }
        [JsonProperty("LastName")]
        public string LastName { get; set; }
    }
}
