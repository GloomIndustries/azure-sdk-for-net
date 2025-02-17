// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace Azure.ResourceManager.RecoveryServicesSiteRecovery.Models
{
    /// <summary> Subnets of the network. </summary>
    public partial class Subnet
    {
        /// <summary> Initializes a new instance of Subnet. </summary>
        internal Subnet()
        {
            AddressList = new ChangeTrackingList<string>();
        }

        /// <summary> Initializes a new instance of Subnet. </summary>
        /// <param name="name"> The subnet name. </param>
        /// <param name="friendlyName"> The subnet friendly name. </param>
        /// <param name="addressList"> The list of addresses for the subnet. </param>
        internal Subnet(string name, string friendlyName, IReadOnlyList<string> addressList)
        {
            Name = name;
            FriendlyName = friendlyName;
            AddressList = addressList;
        }

        /// <summary> The subnet name. </summary>
        public string Name { get; }
        /// <summary> The subnet friendly name. </summary>
        public string FriendlyName { get; }
        /// <summary> The list of addresses for the subnet. </summary>
        public IReadOnlyList<string> AddressList { get; }
    }
}
