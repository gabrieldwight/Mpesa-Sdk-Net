using System.ComponentModel;

namespace MpesaSdk.Enums
{
	/// <summary>
	/// This is a format representing the Mpesa QR Code Output.
	/// </summary>
	public enum QRFormat
	{
		[Description("Image Format")]
		ImageFormat = 1,
		[Description("QR String Format")]
		QRStringFormat = 2,
		[Description("Binary Data Format")]
		BinaryDataFormat = 3,
		[Description("PDF Format")]
		PDFFormat = 4
	}
}
