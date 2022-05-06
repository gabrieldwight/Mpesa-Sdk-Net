using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MpesaSdk.Web.ViewModels
{
    public class DynamicQRViewModel
    {
        public string MerchantName { get; set; }
        public string Reference { get; set; }
        public string CPI { get; set; }
        public int Amount { get; set; }
        public string SelectedQRType { get; set; }
        public List<SelectListItem> QRType { get; set; }
        public string SelectedQRFormat { get; set; }
        public List<SelectListItem> QRFormat { get; set; }
        public string SelectedTransactionCode { get; set; }
        public List<SelectListItem> TransactionCode { get; set; }

        public string GeneratedQRCode { get; set; }

        public string GeneratedQRFileExtension
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(GeneratedQRCode))
                {
                    switch (GeneratedQRCode.ToUpper().Substring(0, 5))
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

        public byte[] GeneratedQRFileImage
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(GeneratedQRCode))
                {
                    return System.Convert.FromBase64String(GeneratedQRCode);
                }
                else
                {
                    return default;
                }
            }
        }
    }
}
