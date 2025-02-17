// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace Azure.ResourceManager.Compute.Models
{
    /// <summary> This is the storage profile of a Gallery Image Version. </summary>
    public partial class SharedGalleryImageVersionStorageProfile
    {
        /// <summary> Initializes a new instance of SharedGalleryImageVersionStorageProfile. </summary>
        internal SharedGalleryImageVersionStorageProfile()
        {
            DataDiskImages = new ChangeTrackingList<SharedGalleryDataDiskImage>();
        }

        /// <summary> Initializes a new instance of SharedGalleryImageVersionStorageProfile. </summary>
        /// <param name="osDiskImage"> This is the OS disk image. </param>
        /// <param name="dataDiskImages"> A list of data disk images. </param>
        internal SharedGalleryImageVersionStorageProfile(SharedGalleryOSDiskImage osDiskImage, IReadOnlyList<SharedGalleryDataDiskImage> dataDiskImages)
        {
            OSDiskImage = osDiskImage;
            DataDiskImages = dataDiskImages;
        }

        /// <summary> This is the OS disk image. </summary>
        public SharedGalleryOSDiskImage OSDiskImage { get; }
        /// <summary> A list of data disk images. </summary>
        public IReadOnlyList<SharedGalleryDataDiskImage> DataDiskImages { get; }
    }
}
