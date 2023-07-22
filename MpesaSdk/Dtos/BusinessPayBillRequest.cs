using MpesaSdk.Enums;
using Newtonsoft.Json;

namespace MpesaSdk.Dtos
{
	public class BusinessPayBillRequest : BusinessToBusiness
	{
		[JsonProperty("Requester")]
		public string Requester { get; private set; }

		public BusinessPayBillRequest(string initiator, string securityCredential, string amount, string partyA, string partyB, string accountReference, string remarks, string queueTimeoutUrl, string resultUrl, string requester)
			: base(initiator, securityCredential, BusinessToBusinessCommandId.BusinessPayBill, amount, partyA, partyB, ((int)IdentifierTypes.Shortcode).ToString(), ((int)IdentifierTypes.Shortcode).ToString(), accountReference, remarks, queueTimeoutUrl, resultUrl)
		{
			Requester = requester;
		}
	}
}
