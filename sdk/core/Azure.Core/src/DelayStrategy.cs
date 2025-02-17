﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#nullable enable

using System;
using Azure.Core.Pipeline;

namespace Azure.Core
{
    /// <summary>
    /// An abstraction to control delay behavior.
    /// </summary>
#pragma warning disable AZC0012 // Avoid single word type names
    public abstract class DelayStrategy
#pragma warning restore AZC0012 // Avoid single word type names
    {
        private readonly Random _random = new ThreadSafeRandom();
        private readonly double _minJitterFactor;
        private readonly double _maxJitterFactor;
        private readonly TimeSpan _maxDelay;

        /// <summary>
        /// Constructs a new instance of <see cref="DelayStrategy"/>.
        /// </summary>
        /// <param name="maxDelay">The max delay value to apply on an individual delay.</param>
        /// <param name="jitterFactor">The jitter factor to apply to each delay. For example, if the delay is 1 second with a jitterFactor of 0.2, the actual
        /// delay used will be a random double between 0.8 and 1.2. If set to 0, no jitter will be applied.</param>
        protected DelayStrategy(TimeSpan? maxDelay = default, double jitterFactor = 0.2)
        {
            // use same defaults as RetryOptions
            _minJitterFactor = 1 - jitterFactor;
            _maxJitterFactor = 1 + jitterFactor;
            _maxDelay = maxDelay ?? TimeSpan.FromMinutes(1);
        }

        /// <summary>
        /// Create an exponential delay with the default jitter factor.
        /// </summary>
        /// <param name="initialDelay">The initial delay to use.</param>
        /// <param name="maxDelay">The maximum delay to use.</param>
        /// <returns>The <see cref="DelayStrategy"/> instance.</returns>
        public static DelayStrategy CreateExponentialDelayStrategy(
            TimeSpan? initialDelay = default,
            TimeSpan? maxDelay = default)
        {
            return new ExponentialDelayStrategy(initialDelay ?? TimeSpan.FromSeconds(0.8), maxDelay ?? TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// Create a fixed delay with jitter.
        /// </summary>
        /// <param name="delay">The delay to use.</param>
        /// <returns>The <see cref="DelayStrategy"/> instance.</returns>
        public static DelayStrategy CreateFixedDelayStrategy(
            TimeSpan? delay = default)
        {
            return new FixedDelayStrategy(delay ?? TimeSpan.FromSeconds(0.8));
        }

        /// <summary>
        /// Gets the next delay interval.
        /// </summary>
        /// <param name="response">The response, if any, returned from the service.</param>
        /// <param name="retryNumber">The retry number.</param>
        /// <returns>A <see cref="TimeSpan"/> representing the next delay interval.</returns>
        protected abstract TimeSpan GetNextDelayCore(Response? response, int retryNumber);

        /// <summary>
        /// Gets the next delay interval taking into account the Max Delay, jitter, and any Retry-After headers.
        /// </summary>
        /// <param name="response">The response, if any, returned from the service.</param>
        /// <param name="retryNumber">The retry number.</param>
        /// <returns>A <see cref="TimeSpan"/> representing the next delay interval.</returns>
        public TimeSpan GetNextDelay(Response? response, int retryNumber) =>
            Max(
                response?.Headers.RetryAfter ?? TimeSpan.Zero,
                Min(
                    ApplyJitter(GetNextDelayCore(response, retryNumber)),
                    _maxDelay));

        private TimeSpan ApplyJitter(TimeSpan delay)
        {
            return TimeSpan.FromMilliseconds(_random.Next((int)(delay.TotalMilliseconds * _minJitterFactor), (int)(delay.TotalMilliseconds * _maxJitterFactor)));
        }

        /// <summary>
        /// Gets the maximum of two <see cref="TimeSpan"/> values.
        /// </summary>
        /// <param name="val1">The first value.</param>
        /// <param name="val2">The second value.</param>
        /// <returns>The maximum of the two <see cref="TimeSpan"/> values.</returns>
        protected static TimeSpan Max(TimeSpan val1, TimeSpan val2) => val1 > val2 ? val1 : val2;

        /// <summary>
        /// Gets the minimum of two <see cref="TimeSpan"/> values.
        /// </summary>
        /// <param name="val1">The first value.</param>
        /// <param name="val2">The second value.</param>
        /// <returns>The minimum of the two <see cref="TimeSpan"/> values.</returns>
        protected static TimeSpan Min(TimeSpan val1, TimeSpan val2) => val1 < val2 ? val1 : val2;
    }
}
