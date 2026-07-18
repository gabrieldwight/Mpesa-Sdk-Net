using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class BundleOfferPurchaseRequest
	{
		[JsonPropertyName("offeringId")]
		public string OfferingId { get; set; }

		[JsonPropertyName("accountId")]
		public string AccountId { get; set; }

		[JsonPropertyName("price")]
		public string Price { get; set; }

		[JsonPropertyName("resourceAmount")]
		public string ResourceAmount { get; set; }

		[JsonPropertyName("validity")]
		public string Validity { get; set; }

		[JsonPropertyName("msisdn")]
		public string Msisdn { get; set; }

		[JsonPropertyName("transactionId")]
		public string TransactionId { get; set; }

		[JsonPropertyName("paymentMode")]
		public string PaymentMode { get; set; }
	}
}
