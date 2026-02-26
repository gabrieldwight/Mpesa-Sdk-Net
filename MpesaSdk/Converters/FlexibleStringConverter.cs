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
				JsonTokenType.Number => reader.GetInt64().ToString(),
				JsonTokenType.True => "true",
				JsonTokenType.False => "false",
				JsonTokenType.Null => null,
				_ => throw new JsonException($"Cannot convert {reader.TokenType} to string.")
			};
		}

		public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) =>
			writer.WriteStringValue(value);
	}
}
