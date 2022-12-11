using Newtonsoft.Json;

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
		[JsonProperty("MerchantName")]
		public string MerchantName { get; private set; }

		/// <summary>
		/// Transaction Reference
		/// </summary>
		[JsonProperty("RefNo")]
		public string RefNo { get; private set; }

		/// <summary>
		/// The total amount for the sale/transaction
		/// </summary>
		[JsonProperty("Amount")]
		public int Amount { get; private set; }

		/// <summary>
		/// Transaction Type. The supported types are
		/// </summary>
		[JsonProperty("TrxCode")]
		public string TrxCode { get; private set; }

		/// <summary>
		/// Credit Party Identifier. Can be a Mobile Number, Business Number, Agent Till, Paybill or Business number, Merchant Buy Goods.
		/// </summary>
		[JsonProperty("CPI")]
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