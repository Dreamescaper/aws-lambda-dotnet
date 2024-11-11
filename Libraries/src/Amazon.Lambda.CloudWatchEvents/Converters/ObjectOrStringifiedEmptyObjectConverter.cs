#if NETCOREAPP3_1_OR_GREATER

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.Lambda.CloudWatchEvents.Converters
{
#if NET8_0_OR_GREATER
    [UnconditionalSuppressMessage("Trimming", "IL2026",
        Justification = "Code in this class will work with AOT if JsonSerializerContext is used.")]

    [UnconditionalSuppressMessage("Trimming", "IL2067",
        Justification = "Code in this class will work with AOT if JsonSerializerContext is used.")]
#endif
    public class ObjectOrStringifiedEmptyObjectConverter<T> : JsonConverter<T>
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String && reader.GetString() == "{}")
            {
#if NET8_0_OR_GREATER
                if (options.TryGetTypeInfo(typeToConvert, out var typeInfo))
                {
                    return (T)typeInfo.CreateObject();
                }
#endif

                return Activator.CreateInstance<T>();
            }

            return JsonSerializer.Deserialize<T>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
#endif