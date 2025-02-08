using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class BillManagerInvoicingRequest
	{
		[JsonPropertyName("externalReference")]
		[JsonInclude]
		public string ExternalReference { get; private set; }

		[JsonPropertyName("billedFullName")]
		[JsonInclude]
		public string BilledFullName { get; private set; }

		[JsonPropertyName("billedPhoneNumber")]
		[JsonInclude]
		public string BilledPhoneNumber { get; private set; }

		[JsonPropertyName("billedPeriod")]
		[JsonInclude]
		public string BilledPeriod { get; private set; }

		[JsonPropertyName("invoiceName")]
		[JsonInclude]
		public string InvoiceName { get; private set; }

		[JsonPropertyName("dueDate")]
		[JsonInclude]
		public string DueDate { get; private set; }

		[JsonPropertyName("accountReference")]
		[JsonInclude]
		public string AccountReference { get; private set; }

		[JsonPropertyName("amount")]
		[JsonInclude]
		public string Amount { get; private set; }

		[JsonPropertyName("invoiceItems")]
		[JsonInclude]
		public List<InvoiceItem> InvoiceItems { get; private set; }

		public BillManagerInvoicingRequest(string externalReference, string billedFullName, string billedPhoneNumber, string billedPeriod, string invoiceName, DateTime dueDate, string accountReference, string amount, List<InvoiceItem> invoiceItems)
		{
			ExternalReference = externalReference;
			BilledFullName = billedFullName;
			BilledPhoneNumber = billedPhoneNumber;
			BilledPeriod = billedPeriod;
			InvoiceName = invoiceName;
			DueDate = dueDate.ToString("yyyy-MM-dd HH:mm:ss.ff");
			AccountReference = accountReference;
			Amount = amount;
			InvoiceItems = invoiceItems;
		}
	}

	public class InvoiceItem
	{
		[JsonPropertyName("itemName")]
		public string ItemName { get; set; }

		[JsonPropertyName("amount")]
		public string Amount { get; set; }
	}
}
