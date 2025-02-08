using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	/// <summary>
	/// Dynamic Mpesa QR data transfer object
	/// </summary>
	public class DynamicMpesaQR
	{
		/// <summary>
		/// Name of the Company/M-Pesa Merchant Name
		/// </summary>
		[JsonPropertyName("MerchantName")]
		[JsonInclude]
		public string MerchantName { get; private set; }

		/// <summary>
		/// Transaction Reference
		/// </summary>
		[JsonPropertyName("RefNo")]
		[JsonInclude]
		public string RefNo { get; private set; }

		/// <summary>
		/// The total amount for the sale/transaction
		/// </summary>
		[JsonPropertyName("Amount")]
		[JsonInclude]
		public int Amount { get; private set; }

		/// <summary>
		/// Transaction Type. The supported types are
		/// </summary>
		[JsonPropertyName("TrxCode")]
		[JsonInclude]
		public string TrxCode { get; private set; }

		/// <summary>
		/// Credit Party Identifier. Can be a Mobile Number, Business Number, Agent Till, Paybill or Business number, Merchant Buy Goods.
		/// </summary>
		[JsonPropertyName("CPI")]
		[JsonInclude]
		public string CPI { get; private set; }

		public DynamicMpesaQR(string merchantName, string refNo, int amount, string trxCode, string cpi)
        {
			MerchantName = merchantName;
			RefNo = refNo;
			Amount = amount;
			TrxCode = trxCode;
			CPI = cpi;
        }

	}
}