using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MpesaSdk.Dtos
{
	public class BillManagerInvoicingRequest
	{
		[JsonProperty("externalReference")]
		public string ExternalReference { get; private set; }

		[JsonProperty("billedFullName")]
		public string BilledFullName { get; private set; }

		[JsonProperty("billedPhoneNumber")]
		public string BilledPhoneNumber { get; private set; }

		[JsonProperty("billedPeriod")]
		public string BilledPeriod { get; private set; }

		[JsonProperty("invoiceName")]
		public string InvoiceName { get; private set; }

		[JsonProperty("dueDate")]
		public string DueDate { get; private set; }

		[JsonProperty("accountReference")]
		public string AccountReference { get; private set; }

		[JsonProperty("amount")]
		public string Amount { get; private set; }

		[JsonProperty("invoiceItems")]
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
		[JsonProperty("itemName")]
		public string ItemName { get; set; }

		[JsonProperty("amount")]
		public string Amount { get; set; }
	}
}
