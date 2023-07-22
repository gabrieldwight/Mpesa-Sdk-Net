using Newtonsoft.Json;
using System;

namespace MpesaSdk.Dtos
{
	public class BillManagerPaymentReconcilliationRequest
	{
		[JsonProperty("transactionId")]
		public string TransactionId { get; private set; }

		[JsonProperty("paidAmount")]
		public string PaidAmount { get; private set; }

		[JsonProperty("msisdn")]
		public string Msisdn { get; private set; }

		[JsonProperty("dateCreated")]
		public string DateCreated { get; private set; }

		[JsonProperty("accountReference")]
		public string AccountReference { get; private set; }

		[JsonProperty("shortCode")]
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
