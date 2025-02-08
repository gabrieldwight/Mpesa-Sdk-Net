using MpesaSdk.Enums;
using System;
using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class StandingOrderRequest
	{
		[JsonPropertyName("StandingOrderName")]
		public string StandingOrderName { get; set; }

		[JsonPropertyName("StartDate")]
		public string StartDate { get; set; }

		[JsonPropertyName("EndDate")]
		public string EndDate { get; set; }

		[JsonPropertyName("BusinessShortCode")]
		public string BusinessShortCode { get; set; }

		[JsonPropertyName("TransactionType")]
		public string TransactionType { get; set; }

		[JsonPropertyName("ReceiverPartyIdentifierType")]
		public string ReceiverPartyIdentifierType { get; set; }

		[JsonPropertyName("Amount")]
		public string Amount { get; set; }

		[JsonPropertyName("PartyA")]
		public string PartyA { get; set; }

		[JsonPropertyName("CallBackURL")]
		public string CallBackUrl { get; set; }

		[JsonPropertyName("AccountReference")]
		public string AccountReference { get; set; }

		[JsonPropertyName("TransactionDesc")]
		public string TransactionDesc { get; set; }

		[JsonPropertyName("Frequency")]
		public string Frequency { get; set; }

		/// <summary>
		/// The Standing Order APIs enable teams to integrate with the standing order solution by initiating a request to create a standing order on the customer profile.
		/// </summary>
		/// <param name="standingOrderName"></param>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <param name="businessShortCode"></param>
		/// <param name="transactionType"></param>
		/// <param name="receiverPartyIdentifierType"></param>
		/// <param name="amount"></param>
		/// <param name="partyA"></param>
		/// <param name="callBackUrl"></param>
		/// <param name="accountReference"></param>
		/// <param name="transactionDesc"></param>
		/// <param name="frequency"></param>
		public StandingOrderRequest(string standingOrderName, DateTime startDate, DateTime endDate, string businessShortCode, string transactionType, IdentifierTypes receiverPartyIdentifierType, string amount, string partyA, string callBackUrl, string accountReference, string transactionDesc, FrequencyTypes frequency)
		{
			StandingOrderName = standingOrderName;
			StartDate = startDate.ToString("yyyyMMdd");
			EndDate = endDate.ToString("yyyyMMdd");
			BusinessShortCode = businessShortCode;
			TransactionType = transactionType;
			ReceiverPartyIdentifierType = ((int)receiverPartyIdentifierType).ToString();
			Amount = amount;
			PartyA = partyA;
			CallBackUrl = callBackUrl;
			AccountReference = accountReference;
			TransactionDesc = transactionDesc;
			Frequency = ((int)frequency).ToString();
		}
	}
}
