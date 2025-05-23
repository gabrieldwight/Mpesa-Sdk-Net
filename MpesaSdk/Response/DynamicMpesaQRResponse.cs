﻿using System.Text.Json.Serialization;

namespace MpesaSdk.Response
{
	public class DynamicMpesaQRResponse
	{
		[JsonPropertyName("ResponseCode")]
		public string ResponseCode { get; set; }

		[JsonPropertyName("RequestID")]
		public string RequestID { get; set; }

		[JsonPropertyName("ResponseDescription")]
		public string ResponseDescription { get; set; }

		[JsonPropertyName("QRCode")]
		public string QRCode { get; set; }

		[JsonIgnore]
		public string QRFileExtension
        {
			get
            {
				if (!string.IsNullOrWhiteSpace(QRCode))
                {
					switch (QRCode.ToUpper().Substring(0, 5))
                    {
						case "IVBOR":
							return "png";
						case "JVBER":
							return "pdf";
						default:
							return string.Empty;
                    }
                }
				return string.Empty;
            }
        }

		[JsonIgnore]
		public byte[] QRFileImage
        {
            get
            {
				if (!string.IsNullOrWhiteSpace(QRCode))
				{
					return System.Convert.FromBase64String(QRCode);
				}
                else
                {
					return default;
                }
			}
        }
	}
}