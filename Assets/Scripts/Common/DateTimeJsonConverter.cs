using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DateTimeJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.GetString()! == null) return DateTime.MinValue;
        return DateTime.ParseExact(reader.GetString()!,
            "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTime dateTimeValue,
        JsonSerializerOptions options) =>
        writer.WriteStringValue(dateTimeValue.ToString(
            "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
}