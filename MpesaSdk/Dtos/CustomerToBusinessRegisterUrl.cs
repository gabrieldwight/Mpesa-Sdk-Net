using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
    /// <summary>
    /// C2B Register URLs data transfer object
    /// </summary>
    public class CustomerToBusinessRegisterUrl
    {
        /// <summary>
        /// The short code of the organization. 
        /// </summary>      
        [JsonPropertyName("ShortCode")]
		[JsonInclude]
		public string ShortCode { get; private set; }

        /// <summary>
        /// This parameter specifies what is to happen if for any reason the validation URL is nor reachable. 
        /// Note that, This is the default action value that determines what MPesa will do in the scenario that your
        /// endpoint is unreachable or is unable to respond on time. 
        /// Only two values are allowed: Completed or Cancelled. 
        /// Completed means MPesa will automatically complete your transaction, in the event MPesa is unable to 
        /// reach your Validation URL 
        /// Cancelled means MPesa will automatically cancel the transaction, in the event MPesa is unable to 
        /// reach your Validation URL.
        /// </summary>
        [JsonPropertyName("ResponseType")]
		[JsonInclude]
		public string ResponseType { get; private set; }

        /// <summary>
        /// This is the URL that receives the confirmation request from API upon payment completion.
        /// </summary>
        [JsonPropertyName("ConfirmationURL")]
		[JsonInclude]
		public string ConfirmationURL { get; private set; }

        /// <summary>
        /// This is the URL that receives the validation request from API upon payment submission. 
        /// The validation URL is only called if the external validation on the registered shortcode is enabled. 
        /// (By default External Validation is dissabled, contact MPESA API team if you want this enbaled for your app)
        /// </summary>
        [JsonPropertyName("ValidationURL")]
		[JsonInclude]
		public string ValidationURL { get; private set; }

        /// <summary>
        /// C2B Register URLs data transfer object
        /// </summary>
        /// <param name="shortCode">The short code of the organization. </param>
        /// <param name="responseType">
        /// This parameter specifies what is to happen if for any reason the validation URL is nor reachable. 
        /// Note that, This is the default action value that determines what MPesa will do in the scenario that your
        /// endpoint is unreachable or is unable to respond on time. 
        /// Only two values are allowed: Completed or Cancelled. 
        /// Completed means MPesa will automatically complete your transaction, in the event MPesa is unable to 
        /// reach your Validation URL 
        /// Cancelled means MPesa will automatically cancel the transaction, in the event MPesa is unable to 
        /// reach your Validation URL.
        /// </param>
        /// <param name="confirmationUrl">This is the URL that receives the confirmation request from API upon payment completion.</param>
        /// <param name="validationUrl">
        /// This is the URL that receives the validation request from API upon payment submission. 
        /// The validation URL is only called if the external validation on the registered shortcode is enabled. 
        /// (By default External Validation is dissabled, contact MPESA API team if you want this enbaled for your app)
        /// </param>
        public CustomerToBusinessRegisterUrl(string shortCode, string responseType, string confirmationUrl, string validationUrl)
        {
            ShortCode = shortCode;
            ResponseType = responseType;
            ConfirmationURL = confirmationUrl;
            ValidationURL = validationUrl;
        }
    }
}
