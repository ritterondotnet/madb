﻿// <copyright file="DeviceEventArgs.cs" company="The Android Open Source Project, Ryan Conrad, Quamotion">
// Copyright (c) The Android Open Source Project, Ryan Conrad, Quamotion. All rights reserved.
// </copyright>

namespace Managed.Adb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

        public class DeviceEventArgs : EventArgs
        {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceEventArgs"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public DeviceEventArgs(IDevice device)
        {
            this.Device = device;
        }

        /// <summary>
        /// Gets the device.
        /// </summary>
        /// <value>The device.</value>
        public IDevice Device { get; private set; }
    }
}