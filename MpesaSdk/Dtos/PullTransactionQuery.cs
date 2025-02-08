using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
    public class PullTransactionQuery
    {
        /// <summary>
        /// This is your paybill number/till number, which you expect to receive payments notifications about.
        /// </summary>
        [JsonPropertyName("ShortCode")]
        public string ShortCode { get; set; }

        /// <summary>
        /// The start period of the missing transactions in the format of 2019-07-31 20:35:21 / 2019-07-31 19:00
        /// </summary>
        [JsonPropertyName("StartDate")]
        public string StartDate { get; set; }

        /// <summary>
        /// The end of the period for the missing transactions in the format of 2019-07-31 20:35:21 / 2019-07-31 22:35
        /// </summary>
        [JsonPropertyName("EndDate")]
        public string EndDate { get; set; }

        /// <summary>
        /// Starts from 0. The service uses offset as opposed to page numbers. The OFF SET value allows you to specify which row to start from retrieving data.
        /// Suppose you wanted to show results 101-200. With the OFFSET keyword you type the (page number/index/offset value) 100.
        /// </summary>
        [JsonPropertyName("OffSetValue")]
        public string OffSetValue { get; set; }

        /// <summary>
        /// Pull Transaction Query data transfer object
        /// </summary>
        /// <param name="shortCode">
        /// Organization ShortCode that was used during Go-Live process.
        /// </param>
        /// <param name="startDate">
        /// The start period of the missing transactions in the format of 2019-07-31 20:35:21 / 2019-07-31 19:00
        /// </param>
        /// <param name="endDate">
        /// The end of the period for the missing transactions in the format of 2019-07-31 20:35:21 / 2019-07-31 22:35
        /// </param>
        /// <param name="offSetValue">
        /// Starts from 0. The service uses offset as opposed to page numbers. The OFF SET value allows you to specify which row to start from retrieving data.
        /// Suppose you wanted to show results 101-200. With the OFFSET keyword you type the (page number/index/offset value) 100.
        /// </param>
        public PullTransactionQuery(string shortCode, string startDate, string endDate, string offSetValue)
        {
            ShortCode = shortCode;
            StartDate = startDate;
            EndDate = endDate;
            OffSetValue = offSetValue;
        }
    }
}
