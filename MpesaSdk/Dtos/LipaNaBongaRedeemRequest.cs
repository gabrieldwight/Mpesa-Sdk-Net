using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class LipaNaBongaRedeemRequest
	{
		[JsonPropertyName("msisdn")]
		public string Msisdn { get; set; }

		[JsonPropertyName("amount")]
		public int Amount { get; set; }

		[JsonPropertyName("bongaPoints")]
		public int BongaPoints { get; set; }

		[JsonPropertyName("conversionRate")]
		public decimal ConversionRate { get; set; }

		[JsonPropertyName("shortCode")]
		public string ShortCode { get; set; }

		[JsonPropertyName("accountNumber")]
		public string AccountNumber { get; set; }
	}
}
