using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MpesaSdk.Converters
{
	internal sealed class FlexibleStringConverter : JsonConverter<string>
	{
		public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return reader.TokenType switch
			{
				JsonTokenType.String => reader.GetString(),
				JsonTokenType.Number => reader.TryGetInt64(out long lVal) ? lVal.ToString() : reader.GetDecimal().ToString(),
				JsonTokenType.True => "true",
				JsonTokenType.False => "false",
				JsonTokenType.Null => string.Empty,
				JsonTokenType.None => string.Empty,
				// Skip inner arrays or objects if Safaricom sends complex nested tokens for a value
				JsonTokenType.StartArray => SkipAndReturnEmpty(ref reader),
				JsonTokenType.StartObject => SkipAndReturnEmpty(ref reader),
				_ => throw new JsonException($"Cannot convert {reader.TokenType} to string.")
			};
		}

		public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) =>
			writer.WriteStringValue(value);

		private static string SkipAndReturnEmpty(ref Utf8JsonReader reader)
		{
			using var doc = JsonDocument.ParseValue(ref reader);
			return doc.RootElement.GetRawText();
		}
	}
}
