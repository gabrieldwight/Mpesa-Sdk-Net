using Newtonsoft.Json;

namespace MpesaSdk.Response
{
    public class MpesaBaseResponse
    {
        /// <summary>
        /// The unique request ID returned by mpesa for each request made
        /// </summary>
        /// <value>The conversation identifier.</value>
        [JsonProperty("ConversationID")]
        public string ConversationId { get; set; }

        /// <summary>
        /// The unique request ID for tracking a transaction
        /// </summary>
        /// <value>The originator coversation identifier.</value>
        [JsonProperty("OriginatorConversationID")]
        public string OriginatorCoversationId { get; set; }

        /// <summary>
        /// Response Description message
        /// </summary>
        /// <value>The response description.</value>
        [JsonProperty("ResponseDescription")]
        public string ResponseDescription { get; set; }

        /// <summary>
        /// Gets or Set the Request Id
        /// </summary>
        /// <value>Generated Id from the application implementation used to track the response</value>
        public string RequestId { get; set; }
    }
}
