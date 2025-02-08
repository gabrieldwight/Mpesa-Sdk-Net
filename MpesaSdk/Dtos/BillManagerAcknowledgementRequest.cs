using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class BillManagerAcknowledgementRequest
	{
		[JsonPropertyName("paymentDate")]
		public string PaymentDate { get; set; }

		[JsonPropertyName("paidAmount")]
		public string PaidAmount { get; set; }

		[JsonPropertyName("accountReference")]
		public string AccountReference { get; set; }

		[JsonPropertyName("transactionId")]
		public string TransactionId { get; set; }

		[JsonPropertyName("phoneNumber")]
		public string PhoneNumber { get; set; }

		[JsonPropertyName("fullName")]
		public string FullName { get; set; }

		[JsonPropertyName("invoiceName")]
		public string InvoiceName { get; set; }

		[JsonPropertyName("externalReference")]
		public string ExternalReference { get; set; }
	}
}
