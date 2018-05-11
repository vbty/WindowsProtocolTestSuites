﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// Convert HostName or Ip to IpAddress
    /// </summary>
    public static class IpUtility
    {
        /// <summary>
        /// Parse hostname or ip string to IPAddress
        /// </summary>
        /// <param name="hostNameOrIpAddress">HostName or Ip String</param>
        /// <returns>IPV4 Address</returns>
        public static IPAddress ParseIPAddress(this string hostNameOrIpAddress)
        {
            IPAddress ipAddress;

            if (string.IsNullOrEmpty(hostNameOrIpAddress))
            {
                return IPAddress.None;
            }

            bool isIp = IPAddress.TryParse(hostNameOrIpAddress, out ipAddress);
            if (!isIp)
            {
                try
                {
                    IPAddress[] ipAddresses = Dns.GetHostAddresses(hostNameOrIpAddress);
                    ipAddress = ipAddresses.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First();
                }
                catch
                {
                    throw new System.Exception(string.Format("Cannot resolve IP address of ({0}) from DNS.", hostNameOrIpAddress));
                }
            }

            return ipAddress;
        }

        /// <summary>
        /// Parse hostname or ip string to IPAddress Arrary
        /// </summary>
        /// <param name="hostNameOrIpAddress">HostName or Ip Address, "," or ";" between each hostname or Ip</param>
        /// <returns>IpAddress Arrary</returns>
        public static IPAddress[] ParseArraryIPAddress(this string hostNameOrIpAddress)
        {
            if (string.IsNullOrEmpty(hostNameOrIpAddress))
            {
                return new IPAddress[0];
            }

            List<IPAddress> ipList = new List<IPAddress>();
            foreach(string nameOrIp in hostNameOrIpAddress.Split(new char[] {',',';' }))
            {
                ipList.Add(nameOrIp.ParseIPAddress());
            }
            return ipList.ToArray();
        }
    }
}