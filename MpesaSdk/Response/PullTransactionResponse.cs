using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MpesaSdk.Response
{
    public class PullTransactionResponse
    {
        [JsonProperty("ResponseRefID")]
        public string ResponseRefID { get; set; }

        [JsonProperty("ResponseCode")]
        public string ResponseCode { get; set; }

        [JsonProperty("ResponseMessage")]
        public string ResponseMessage { get; set; }

        [JsonProperty("Response")]
        public List<List<PullTransactionDetails>> Response { get; set; }
    }

    public class PullTransactionDetails
    {
        [JsonProperty("transactionId")]
        public string transactionId { get; set; }

        [JsonProperty("trxDate")]
        public DateTime trxDate { get; set; }

        [JsonProperty("msisdn")]
        public int msisdn { get; set; }

        [JsonProperty("sender")]
        public string sender { get; set; }

        [JsonProperty("transactiontype")]
        public string transactiontype { get; set; }

        [JsonProperty("billreference")]
        public string billreference { get; set; }

        [JsonProperty("amount")]
        public string amount { get; set; }

        [JsonProperty("organizationname")]
        public string organizationname { get; set; }
    }
}
