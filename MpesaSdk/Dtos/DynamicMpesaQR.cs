using Newtonsoft.Json;

namespace MpesaSdk.Dtos
{
	/// <summary>
	/// Dynamic Mpesa QR data transfer object
	/// </summary>
	public class DynamicMpesaQR
	{
		/// <summary>
		/// Version number of the QR.
		/// </summary>
		[JsonProperty("QRVersion")]
		public string QRVersion { get; private set; }

		/// <summary>
		/// Format of QR output:
		/// </summary>
		[JsonProperty("QRFormat")]
		public int QRFormat { get; private set; }

		/// <summary>
		/// The type of QR being used
		/// </summary>
		[JsonProperty("QRType")]
		public string QRType { get; private set; }

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

		public DynamicMpesaQR(string qrVersion, int qrFormat, string qrType, string merchantName, string refNo, int amount, string trxCode, string cpi)
        {
			QRVersion = qrVersion;
			QRFormat = qrFormat;
			QRType = qrType;
			MerchantName = merchantName;
			RefNo = refNo;
			Amount = amount;
			TrxCode = trxCode;
			CPI = cpi;
        }

	}
}
