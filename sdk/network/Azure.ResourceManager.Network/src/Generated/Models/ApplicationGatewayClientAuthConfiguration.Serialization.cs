// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.Network.Models
{
    internal partial class ApplicationGatewayClientAuthConfiguration : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(VerifyClientCertIssuerDN))
            {
                writer.WritePropertyName("verifyClientCertIssuerDN"u8);
                writer.WriteBooleanValue(VerifyClientCertIssuerDN.Value);
            }
            writer.WriteEndObject();
        }

        internal static ApplicationGatewayClientAuthConfiguration DeserializeApplicationGatewayClientAuthConfiguration(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            Optional<bool> verifyClientCertIssuerDN = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("verifyClientCertIssuerDN"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    verifyClientCertIssuerDN = property.Value.GetBoolean();
                    continue;
                }
            }
            return new ApplicationGatewayClientAuthConfiguration(Optional.ToNullable(verifyClientCertIssuerDN));
        }
    }
}
