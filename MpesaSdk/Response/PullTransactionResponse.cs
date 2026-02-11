using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MpesaSdk.Response
{
    public class PullTransactionResponse
    {
        [JsonPropertyName("ResponseRefID")]
        public string ResponseRefID { get; set; }

        [JsonPropertyName("ResponseCode")]
        public string ResponseCode { get; set; }

        [JsonPropertyName("ResponseMessage")]
        public string ResponseMessage { get; set; }

        [JsonPropertyName("Response")]
        public List<List<PullTransactionDetails>> Response { get; set; }
    }

    public class PullTransactionDetails
    {
        [JsonPropertyName("transactionId")]
        public string transactionId { get; set; }

        [JsonPropertyName("trxDate")]
        public DateTime trxDate { get; set; }

        [JsonPropertyName("msisdn")]
        public string msisdn { get; set; }

        [JsonPropertyName("sender")]
        public string sender { get; set; }

        [JsonPropertyName("transactiontype")]
        public string transactiontype { get; set; }

        [JsonPropertyName("billreference")]
        public string billreference { get; set; }

        [JsonPropertyName("amount")]
        public string amount { get; set; }

        [JsonPropertyName("organizationname")]
        public string organizationname { get; set; }
    }
}
