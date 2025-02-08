using System;
using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class BillManagerPaymentReconcilliationRequest
	{
		[JsonPropertyName("transactionId")]
		[JsonInclude]
		public string TransactionId { get; private set; }

		[JsonPropertyName("paidAmount")]
		[JsonInclude]
		public string PaidAmount { get; private set; }

		[JsonPropertyName("msisdn")]
		[JsonInclude]
		public string Msisdn { get; private set; }

		[JsonPropertyName("dateCreated")]
		[JsonInclude]
		public string DateCreated { get; private set; }

		[JsonPropertyName("accountReference")]
		[JsonInclude]
		public string AccountReference { get; private set; }

		[JsonPropertyName("shortCode")]
		[JsonInclude]
		public string ShortCode { get; private set; }

		public BillManagerPaymentReconcilliationRequest(string transactionId, string paidAmount, string msisdn, DateTime dateCreated, string accountReference, string shortCode)
		{
			TransactionId = transactionId;
			PaidAmount = paidAmount;
			Msisdn = msisdn;
			DateCreated = dateCreated.ToString("yyyy-MM-dd");
			AccountReference = accountReference;
			ShortCode = shortCode;
		}
	}
}
