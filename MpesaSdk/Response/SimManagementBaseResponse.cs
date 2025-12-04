using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MpesaSdk.Response
{
	public class SimManagementBaseResponse
	{
		[JsonPropertyName("header")]
		public Header Header { get; set; }

		[JsonPropertyName("body")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Body Body { get; set; }
	}

	public class Body
	{
		[JsonPropertyName("desc")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Desc { get; set; }

		[JsonPropertyName("status")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Status { get; set; }

		[JsonPropertyName("statusCode")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string StatusCode { get; set; }

		[JsonPropertyName("offeringName")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string OfferingName { get; set; }

		[JsonPropertyName("offeringStatus")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string OfferingStatus { get; set; }

		[JsonPropertyName("subscriberStatus")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string SubscriberStatus { get; set; }

		[JsonPropertyName("offeringId")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string OfferingId { get; set; }

		[JsonPropertyName("vpnGroup")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string VpnGroup { get; set; }

		[JsonPropertyName("requestId")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string RequestId { get; set; }

		[JsonPropertyName("ID")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Id { get; set; }

		[JsonPropertyName("Desc")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<Desc> DescList { get; set; }

		[JsonPropertyName("body")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<BodyElement> BodyElement { get; set; }

		[JsonPropertyName("result")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Result { get; set; }

		[JsonPropertyName("statusDesc")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string StatusDesc { get; set; }

		[JsonPropertyName("content")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<Content> Content { get; set; }

		[JsonPropertyName("pageable")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Pageable Pageable { get; set; }

		[JsonPropertyName("totalPages")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int TotalPages { get; set; }

		[JsonPropertyName("totalElements")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int TotalElements { get; set; }

		[JsonPropertyName("last")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public bool Last { get; set; }

		[JsonPropertyName("numberOfElements")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int NumberOfElements { get; set; }

		[JsonPropertyName("size")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int Size { get; set; }

		[JsonPropertyName("number")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int Number { get; set; }

		[JsonPropertyName("sort")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Sort Sort { get; set; }

		[JsonPropertyName("first")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public bool First { get; set; }

		[JsonPropertyName("empty")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public bool Empty { get; set; }
	}

	public class BodyElement
	{
		[JsonPropertyName("pooledTrend")]
		public List<string> PooledTrend { get; set; }

		[JsonPropertyName("suspendedTrend")]
		public List<string> SuspendedTrend { get; set; }

		[JsonPropertyName("dates")]
		public List<string> Dates { get; set; }

		[JsonPropertyName("activeTrend")]
		public List<string> ActiveTrend { get; set; }

		[JsonPropertyName("idleTrend")]
		public List<string> IdleTrend { get; set; }
	}

	public class Desc
	{
		[JsonPropertyName("life_cycle_status")]
		public string LifeCycleStatus { get; set; }

		[JsonPropertyName("iccid")]
		public string Iccid { get; set; }

		[JsonPropertyName("asset_name")]
		public string AssetName { get; set; }

		[JsonPropertyName("activation_date")]
		public string ActivationDate { get; set; }

		[JsonPropertyName("expiry_date")]
		public string ExpiryDate { get; set; }

		[JsonPropertyName("imei")]
		public string Imei { get; set; }

		[JsonPropertyName("product_status")]
		public string ProductStatus { get; set; }

		[JsonPropertyName("imsi")]
		public string Imsi { get; set; }

		[JsonPropertyName("msisdn")]
		public string Msisdn { get; set; }

		[JsonPropertyName("vpn_group")]
		public string VpnGroup { get; set; }

		[JsonPropertyName("activation_agent")]
		public string ActivationAgent { get; set; }
	}

	public class Header
	{
		[JsonPropertyName("requestRefId")]
		public string RequestRefId { get; set; }

		[JsonPropertyName("responseCode")]
		public long ResponseCode { get; set; }

		[JsonPropertyName("responseMessage")]
		public string ResponseMessage { get; set; }

		[JsonPropertyName("customerMessage")]
		public string CustomerMessage { get; set; }

		[JsonPropertyName("timestamp")]
		public DateTimeOffset Timestamp { get; set; }
	}

	public class Content
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("recepitId")]
		public int RecepitId { get; set; }

		[JsonPropertyName("sourceAddr")]
		public string SourceAddr { get; set; }

		[JsonPropertyName("msisdn")]
		public string Msisdn { get; set; }

		[JsonPropertyName("message")]
		public string Message { get; set; }

		[JsonPropertyName("sourceSystem")]
		public string SourceSystem { get; set; }

		[JsonPropertyName("processingStatus")]
		public string ProcessingStatus { get; set; }

		[JsonPropertyName("messageId")]
		public string MessageId { get; set; }

		[JsonPropertyName("date")]
		public string Date { get; set; }

		[JsonPropertyName("deliverTime")]
		public string DeliverTime { get; set; }

		[JsonPropertyName("description")]
		public string Description { get; set; }

		[JsonPropertyName("vpnGroup")]
		public string VpnGroup { get; set; }
	}

	public class Pageable
	{
		[JsonPropertyName("pageNumber")]
		public int PageNumber { get; set; }

		[JsonPropertyName("pageSize")]
		public int PageSize { get; set; }

		[JsonPropertyName("sort")]
		public Sort Sort { get; set; }

		[JsonPropertyName("offset")]
		public int Offset { get; set; }

		[JsonPropertyName("unpaged")]
		public bool Unpaged { get; set; }

		[JsonPropertyName("paged")]
		public bool Paged { get; set; }
	}

	public class Sort
	{
		[JsonPropertyName("unsorted")]
		public bool Unsorted { get; set; }

		[JsonPropertyName("sorted")]
		public bool Sorted { get; set; }

		[JsonPropertyName("empty")]
		public bool Empty { get; set; }
	}
}
