using Newtonsoft.Json;

namespace MpesaSdk.Dtos
{
	public class BillManagerOnboardingRequest
	{
		[JsonProperty("shortcode")]
		public string Shortcode { get; private set; }

		[JsonProperty("email")]
		public string Email { get; private set; }

		[JsonProperty("officialContact")]
		public string OfficialContact { get; private set; }

		[JsonProperty("sendReminders")]
		public string SendReminders { get; private set; }

		[JsonProperty("logo")]
		public string Logo { get; private set; }

		[JsonProperty("callbackurl")]
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
