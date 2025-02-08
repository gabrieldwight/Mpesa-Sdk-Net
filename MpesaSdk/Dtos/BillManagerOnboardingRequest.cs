using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class BillManagerOnboardingRequest
	{
		[JsonPropertyName("shortcode")]
		[JsonInclude]
		public string Shortcode { get; private set; }

		[JsonPropertyName("email")]
		[JsonInclude]
		public string Email { get; private set; }

		[JsonPropertyName("officialContact")]
		[JsonInclude]
		public string OfficialContact { get; private set; }

		[JsonPropertyName("sendReminders")]
		[JsonInclude]
		public string SendReminders { get; private set; }

		[JsonPropertyName("logo")]
		[JsonInclude]
		public string Logo { get; private set; }

		[JsonPropertyName("callbackurl")]
		[JsonInclude]
		public string Callbackurl { get; private set; }

		public BillManagerOnboardingRequest(string shortcode, string email, string officialContact, string sendReminders, string logo, string callbackurl)
		{
			Shortcode = shortcode;
			Email = email;
			OfficialContact = officialContact;
			SendReminders = sendReminders;
			Logo = logo;
			Callbackurl = callbackurl;
		}
	}
}
