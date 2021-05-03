using Newtonsoft.Json;

namespace MpesaSdk.Dtos
{
    public class PullTransactionQuery
    {
        /// <summary>
        /// This is your paybill number/till number, which you expect to receive payments notifications about.
        /// </summary>
        [JsonProperty("ShortCode")]
        public string ShortCode { get; set; }

        /// <summary>
        /// The start period of the missing transactions in the format of 2019-07-31 20:35:21 / 2019-07-31 19:00
        /// </summary>
        [JsonProperty("StartDate")]
        public string StartDate { get; set; }

        /// <summary>
        /// The end of the period for the missing transactions in the format of 2019-07-31 20:35:21 / 2019-07-31 22:35
        /// </summary>
        [JsonProperty("EndDate")]
        public string EndDate { get; set; }

        /// <summary>
        /// Starts from 0. The service uses offset as opposed to page numbers. The OFF SET value allows you to specify which row to start from retrieving data.
        /// Suppose you wanted to show results 101-200. With the OFFSET keyword you type the (page number/index/offset value) 100.
        /// </summary>
        [JsonProperty("OffSetValue")]
        public string OffSetValue { get; set; }
    }
}
