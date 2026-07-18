using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class B2BHakikishaRequest
	{
		[JsonPropertyName("identifierType")]
		public string IdentifierType { get; set; }

		[JsonPropertyName("identifier")]
		public string Identifier { get; set; }
	}
}
