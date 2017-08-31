using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Smart365Operations.Common.Infrastructure.Utility
{
    public class SystemHelper
    {
        public static string GetMACAddress(string mac)
        {
            var nicList = NetworkInterface.GetAllNetworkInterfaces();
            var isExist = nicList.Any(n => !string.IsNullOrEmpty(AddressBytesToString(n.GetPhysicalAddress().GetAddressBytes())) && AddressBytesToString(n.GetPhysicalAddress().GetAddressBytes()).Equals(mac));
            if (isExist)
            {
                return mac;
            }
            else
            {
                var ethernetList = from e in nicList
                                   where IsEthernet(e)
                                   select AddressBytesToString(e.GetPhysicalAddress().GetAddressBytes());
                if (ethernetList == null || ethernetList.Count() == 0)
                {
                    var wirelessList = from w in nicList
                                       where IsWireless(w)
                                       select AddressBytesToString(w.GetPhysicalAddress().GetAddressBytes());
                    if (wirelessList == null || wirelessList.Count() == 0)
                    {
                        var allList = from a in nicList
                                      where !string.IsNullOrEmpty(AddressBytesToString(a.GetPhysicalAddress().GetAddressBytes()))
                                      select AddressBytesToString(a.GetPhysicalAddress().GetAddressBytes());
                        if (allList == null || allList.Count() == 0)
                        {
                            return string.Empty;
                        }
                        else
                        {
                            var orderedAllList = allList.OrderBy(a => a);
                            return orderedAllList.ElementAt(0);
                        }
                    }
                    else
                    {
                        var orderedWirelessList = wirelessList.OrderBy(a => a);
                        return orderedWirelessList.ElementAt(0);
                    }
                }
                else
                {
                    var orderedEthernetList = ethernetList.OrderBy(a => a);
                    string value = orderedEthernetList.ElementAt(0);
                    return value;
                }
            }
            //return string.Empty;
        }


        private static bool IsConnected(NetworkInterface nic)
        {
            return nic.OperationalStatus == OperationalStatus.Up;
        }

        private static bool IsEthernetOrWireless(System.Net.NetworkInformation.NetworkInterface nic)
        {
            return (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet
                                || nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211);
        }

        private static bool IsEthernet(NetworkInterface nic)
        {
            //return nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet;
            if (nic.NetworkInterfaceType != NetworkInterfaceType.Ethernet) return false;
            string key = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\";
            //        if ((nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            //|| (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet3Megabit)
            //|| (nic.NetworkInterfaceType == NetworkInterfaceType.FastEthernetFx)
            //|| (nic.NetworkInterfaceType == NetworkInterfaceType.FastEthernetT)
            //|| (nic.NetworkInterfaceType == NetworkInterfaceType.GigabitEthernet))
            if (nic.GetPhysicalAddress().ToString().Length != 0)
            {
                string fRegisteryKey = key + nic.Id + "\\Connection";
                Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(fRegisteryKey, false);
                if (rk != null)
                {
                    string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();
                    int fMediaSubType = Convert.ToInt32(rk.GetValue("MediaSubType", 0));
                    if (fPnpInstanceID.Length > 3 &&
                        fPnpInstanceID.Substring(0, 3) == "PCI")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsWireless(NetworkInterface nic)
        {
            return nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211;
        }

        private static string AddressBytesToString(byte[] addressBytes)
        {
            return string.Join("", (from b in addressBytes
                                    select b.ToString("X2")).ToArray());
        }
    }
}
