using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MpesaSdk.Response
{
	public class BundleFetchResponse
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("desc")]
		public string Desc { get; set; }

		[JsonPropertyName("status")]
		public string Status { get; set; }

		[JsonPropertyName("relatedSubscription")]
		public RelatedSubscription RelatedSubscription { get; set; }

		[JsonPropertyName("lineItem")]
		public LineItem LineItem { get; set; }
	}

	public class RelatedSubscription
	{
		[JsonPropertyName("desc")]
		public string Desc { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}

	public class LineItem
	{
		[JsonPropertyName("characteristicsValues")]
		public List<CharacteristicsValue> CharacteristicsValues { get; set; }
	}

	public class CharacteristicsValue
	{
		[JsonPropertyName("offerName")]
		public string OfferName { get; set; }

		[JsonPropertyName("uniqueOfferingId")]
		public string UniqueOfferingId { get; set; }

		[JsonPropertyName("offerValidity")]
		public int OfferValidity { get; set; }

		[JsonPropertyName("resourceAccId")]
		public int ResourceAccId { get; set; }

		[JsonPropertyName("resourceValue")]
		public int ResourceValue { get; set; }

		[JsonPropertyName("offerPrice")]
		public int OfferPrice { get; set; }

		[JsonPropertyName("offerUssdName")]
		public string OfferUssdName { get; set; }

		[JsonPropertyName("offeringId")]
		public int OfferingId { get; set; }

		[JsonPropertyName("offerSource")]
		public string OfferSource { get; set; }

		[JsonPropertyName("locationId")]
		public int LocationId { get; set; }

		[JsonPropertyName("subscribed")]
		public int Subscribed { get; set; }

		[JsonPropertyName("childOffers")]
		public List<ChildOffers> ChildOffers { get; set; }
	}

	public class ChildOffers
	{
		[JsonPropertyName("offerName")]
		public string OfferName { get; set; }
		
		[JsonPropertyName("offerValidity")]
		public int OfferValidity { get; set; }

		[JsonPropertyName("resourceAccId")]
		public int ResourceAccId { get; set; }

		[JsonPropertyName("resourceValue")]
		public int ResourceValue { get; set; }

		[JsonPropertyName("offerPrice")]
		public int OfferPrice { get; set; }

		[JsonPropertyName("offerUssdName")]
		public string OfferUssdName { get; set; }

		[JsonPropertyName("parentOfferId")]
		public int ParentOfferId { get; set; }
	}
}
