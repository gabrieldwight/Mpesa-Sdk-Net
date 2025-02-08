using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    /// <summary>
    /// Callback to handle C2B result response
    /// </summary>
    public class CustomerToBusinessCallback
    {
        [JsonPropertyName("TransactionType")]
        public string TransactionType { get; set; }
        [JsonPropertyName("TransID")]
        public string TransID { get; set; }
        [JsonPropertyName("TransTime")]
        public string TransTime { get; set; }
        [JsonPropertyName("TransAmount")]
        public string TransAmount { get; set; }
        [JsonPropertyName("BusinessShortCode")]
        public string BusinessShortCode { get; set; }
        [JsonPropertyName("BillRefNumber")]
        public string BillRefNumber { get; set; }
        [JsonPropertyName("InvoiceNumber")]
        public string InvoiceNumber { get; set; }
        [JsonPropertyName("OrgAccountBalance")]
        public string OrgAccountBalance { get; set; }
        [JsonPropertyName("ThirdPartyTransID")]
        public string ThirdPartyTransID { get; set; }
        [JsonPropertyName("MSISDN")]
        public string MSISDN { get; set; }
        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("MiddleName")]
        public string MiddleName { get; set; }
        [JsonPropertyName("LastName")]
        public string LastName { get; set; }
    }
}
