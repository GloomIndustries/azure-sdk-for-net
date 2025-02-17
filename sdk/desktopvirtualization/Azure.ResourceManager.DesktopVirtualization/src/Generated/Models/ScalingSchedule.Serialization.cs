// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.DesktopVirtualization.Models
{
    public partial class ScalingSchedule : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(Name))
            {
                writer.WritePropertyName("name"u8);
                writer.WriteStringValue(Name);
            }
            if (Optional.IsCollectionDefined(DaysOfWeek))
            {
                writer.WritePropertyName("daysOfWeek"u8);
                writer.WriteStartArray();
                foreach (var item in DaysOfWeek)
                {
                    writer.WriteStringValue(item.ToString());
                }
                writer.WriteEndArray();
            }
            if (Optional.IsDefined(RampUpStartOn))
            {
                writer.WritePropertyName("rampUpStartTime"u8);
                writer.WriteStringValue(RampUpStartOn.Value, "O");
            }
            if (Optional.IsDefined(RampUpLoadBalancingAlgorithm))
            {
                writer.WritePropertyName("rampUpLoadBalancingAlgorithm"u8);
                writer.WriteStringValue(RampUpLoadBalancingAlgorithm.Value.ToString());
            }
            if (Optional.IsDefined(RampUpMinimumHostsPct))
            {
                writer.WritePropertyName("rampUpMinimumHostsPct"u8);
                writer.WriteNumberValue(RampUpMinimumHostsPct.Value);
            }
            if (Optional.IsDefined(RampUpCapacityThresholdPct))
            {
                writer.WritePropertyName("rampUpCapacityThresholdPct"u8);
                writer.WriteNumberValue(RampUpCapacityThresholdPct.Value);
            }
            if (Optional.IsDefined(PeakStartOn))
            {
                writer.WritePropertyName("peakStartTime"u8);
                writer.WriteStringValue(PeakStartOn.Value, "O");
            }
            if (Optional.IsDefined(PeakLoadBalancingAlgorithm))
            {
                writer.WritePropertyName("peakLoadBalancingAlgorithm"u8);
                writer.WriteStringValue(PeakLoadBalancingAlgorithm.Value.ToString());
            }
            if (Optional.IsDefined(RampDownStartOn))
            {
                writer.WritePropertyName("rampDownStartTime"u8);
                writer.WriteStringValue(RampDownStartOn.Value, "O");
            }
            if (Optional.IsDefined(RampDownLoadBalancingAlgorithm))
            {
                writer.WritePropertyName("rampDownLoadBalancingAlgorithm"u8);
                writer.WriteStringValue(RampDownLoadBalancingAlgorithm.Value.ToString());
            }
            if (Optional.IsDefined(RampDownMinimumHostsPct))
            {
                writer.WritePropertyName("rampDownMinimumHostsPct"u8);
                writer.WriteNumberValue(RampDownMinimumHostsPct.Value);
            }
            if (Optional.IsDefined(RampDownCapacityThresholdPct))
            {
                writer.WritePropertyName("rampDownCapacityThresholdPct"u8);
                writer.WriteNumberValue(RampDownCapacityThresholdPct.Value);
            }
            if (Optional.IsDefined(RampDownForceLogoffUsers))
            {
                writer.WritePropertyName("rampDownForceLogoffUsers"u8);
                writer.WriteBooleanValue(RampDownForceLogoffUsers.Value);
            }
            if (Optional.IsDefined(RampDownStopHostsWhen))
            {
                writer.WritePropertyName("rampDownStopHostsWhen"u8);
                writer.WriteStringValue(RampDownStopHostsWhen.Value.ToString());
            }
            if (Optional.IsDefined(RampDownWaitTimeMinutes))
            {
                writer.WritePropertyName("rampDownWaitTimeMinutes"u8);
                writer.WriteNumberValue(RampDownWaitTimeMinutes.Value);
            }
            if (Optional.IsDefined(RampDownNotificationMessage))
            {
                writer.WritePropertyName("rampDownNotificationMessage"u8);
                writer.WriteStringValue(RampDownNotificationMessage);
            }
            if (Optional.IsDefined(OffPeakStartOn))
            {
                writer.WritePropertyName("offPeakStartTime"u8);
                writer.WriteStringValue(OffPeakStartOn.Value, "O");
            }
            if (Optional.IsDefined(OffPeakLoadBalancingAlgorithm))
            {
                writer.WritePropertyName("offPeakLoadBalancingAlgorithm"u8);
                writer.WriteStringValue(OffPeakLoadBalancingAlgorithm.Value.ToString());
            }
            writer.WriteEndObject();
        }

        internal static ScalingSchedule DeserializeScalingSchedule(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            Optional<string> name = default;
            Optional<IList<ScalingScheduleDaysOfWeekItem>> daysOfWeek = default;
            Optional<DateTimeOffset> rampUpStartTime = default;
            Optional<SessionHostLoadBalancingAlgorithm> rampUpLoadBalancingAlgorithm = default;
            Optional<int> rampUpMinimumHostsPct = default;
            Optional<int> rampUpCapacityThresholdPct = default;
            Optional<DateTimeOffset> peakStartTime = default;
            Optional<SessionHostLoadBalancingAlgorithm> peakLoadBalancingAlgorithm = default;
            Optional<DateTimeOffset> rampDownStartTime = default;
            Optional<SessionHostLoadBalancingAlgorithm> rampDownLoadBalancingAlgorithm = default;
            Optional<int> rampDownMinimumHostsPct = default;
            Optional<int> rampDownCapacityThresholdPct = default;
            Optional<bool> rampDownForceLogoffUsers = default;
            Optional<DesktopVirtualizationStopHostsWhen> rampDownStopHostsWhen = default;
            Optional<int> rampDownWaitTimeMinutes = default;
            Optional<string> rampDownNotificationMessage = default;
            Optional<DateTimeOffset> offPeakStartTime = default;
            Optional<SessionHostLoadBalancingAlgorithm> offPeakLoadBalancingAlgorithm = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("name"u8))
                {
                    name = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("daysOfWeek"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<ScalingScheduleDaysOfWeekItem> array = new List<ScalingScheduleDaysOfWeekItem>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(new ScalingScheduleDaysOfWeekItem(item.GetString()));
                    }
                    daysOfWeek = array;
                    continue;
                }
                if (property.NameEquals("rampUpStartTime"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    rampUpStartTime = property.Value.GetDateTimeOffset("O");
                    continue;
                }
                if (property.NameEquals("rampUpLoadBalancingAlgorithm"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    rampUpLoadBalancingAlgorithm = new SessionHostLoadBalancingAlgorithm(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("rampUpMinimumHostsPct"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    rampUpMinimumHostsPct = property.Value.GetInt32();
                    continue;
                }
                if (property.NameEquals("rampUpCapacityThresholdPct"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    rampUpCapacityThresholdPct = property.Value.GetInt32();
                    continue;
                }
                if (property.NameEquals("peakStartTime"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    peakStartTime = property.Value.GetDateTimeOffset("O");
                    continue;
                }
                if (property.NameEquals("peakLoadBalancingAlgorithm"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    peakLoadBalancingAlgorithm = new SessionHostLoadBalancingAlgorithm(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("rampDownStartTime"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    rampDownStartTime = property.Value.GetDateTimeOffset("O");
                    continue;
                }
                if (property.NameEquals("rampDownLoadBalancingAlgorithm"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    rampDownLoadBalancingAlgorithm = new SessionHostLoadBalancingAlgorithm(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("rampDownMinimumHostsPct"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    rampDownMinimumHostsPct = property.Value.GetInt32();
                    continue;
                }
                if (property.NameEquals("rampDownCapacityThresholdPct"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    rampDownCapacityThresholdPct = property.Value.GetInt32();
                    continue;
                }
                if (property.NameEquals("rampDownForceLogoffUsers"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    rampDownForceLogoffUsers = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("rampDownStopHostsWhen"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    rampDownStopHostsWhen = new DesktopVirtualizationStopHostsWhen(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("rampDownWaitTimeMinutes"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    rampDownWaitTimeMinutes = property.Value.GetInt32();
                    continue;
                }
                if (property.NameEquals("rampDownNotificationMessage"u8))
                {
                    rampDownNotificationMessage = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("offPeakStartTime"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    offPeakStartTime = property.Value.GetDateTimeOffset("O");
                    continue;
                }
                if (property.NameEquals("offPeakLoadBalancingAlgorithm"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    offPeakLoadBalancingAlgorithm = new SessionHostLoadBalancingAlgorithm(property.Value.GetString());
                    continue;
                }
            }
            return new ScalingSchedule(name.Value, Optional.ToList(daysOfWeek), Optional.ToNullable(rampUpStartTime), Optional.ToNullable(rampUpLoadBalancingAlgorithm), Optional.ToNullable(rampUpMinimumHostsPct), Optional.ToNullable(rampUpCapacityThresholdPct), Optional.ToNullable(peakStartTime), Optional.ToNullable(peakLoadBalancingAlgorithm), Optional.ToNullable(rampDownStartTime), Optional.ToNullable(rampDownLoadBalancingAlgorithm), Optional.ToNullable(rampDownMinimumHostsPct), Optional.ToNullable(rampDownCapacityThresholdPct), Optional.ToNullable(rampDownForceLogoffUsers), Optional.ToNullable(rampDownStopHostsWhen), Optional.ToNullable(rampDownWaitTimeMinutes), rampDownNotificationMessage.Value, Optional.ToNullable(offPeakStartTime), Optional.ToNullable(offPeakLoadBalancingAlgorithm));
        }
    }
}
