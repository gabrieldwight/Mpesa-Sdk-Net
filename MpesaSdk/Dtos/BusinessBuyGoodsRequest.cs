using MpesaSdk.Enums;
using Newtonsoft.Json;

namespace MpesaSdk.Dtos
{
	public class BusinessBuyGoodsRequest : BusinessToBusiness
	{
		[JsonProperty("Requester")]
		public string Requester { get; private set; }

		public BusinessBuyGoodsRequest(string initiator, string securityCredential, string amount, string partyA, string partyB, string accountReference, string remarks, string queueTimeoutUrl, string resultUrl, string requester) 
			: base(initiator, securityCredential, BusinessToBusinessCommandId.BusinessBuyGoods, amount, partyA, partyB, ((int)IdentifierTypes.Shortcode).ToString(), ((int)IdentifierTypes.Shortcode).ToString(), accountReference, remarks, queueTimeoutUrl, resultUrl)
		{
			Requester = requester;
		}
	}
}
