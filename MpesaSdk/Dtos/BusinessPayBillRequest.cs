using MpesaSdk.Enums;
using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class BusinessPayBillRequest : BusinessToBusiness
	{
		[JsonPropertyName("Requester")]
		[JsonInclude]
		public string Requester { get; private set; }

		public BusinessPayBillRequest(string initiator, string securityCredential, string amount, string partyA, string partyB, string accountReference, string remarks, string queueTimeoutUrl, string resultUrl, string requester)
			: base(initiator, securityCredential, BusinessToBusinessCommandId.BusinessPayBill, amount, partyA, partyB, ((int)IdentifierTypes.Shortcode).ToString(), ((int)IdentifierTypes.Shortcode).ToString(), accountReference, remarks, queueTimeoutUrl, resultUrl)
		{
			Requester = requester;
		}
	}
}
