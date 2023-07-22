using Newtonsoft.Json;

namespace MpesaSdk.Dtos
{
	public class BillManagerAcknowledgementRequest
	{
		[JsonProperty("paymentDate")]
		public string PaymentDate { get; set; }

		[JsonProperty("paidAmount")]
		public string PaidAmount { get; set; }

		[JsonProperty("accountReference")]
		public string AccountReference { get; set; }

		[JsonProperty("transactionId")]
		public string TransactionId { get; set; }

		[JsonProperty("phoneNumber")]
		public string PhoneNumber { get; set; }

		[JsonProperty("fullName")]
		public string FullName { get; set; }

		[JsonProperty("invoiceName")]
		public string InvoiceName { get; set; }

		[JsonProperty("externalReference")]
		public string ExternalReference { get; set; }
	}
}
