using System;
using System.Collections.Generic;
using System.Management;

namespace adapman
{
    public static class NetworkAdapterManager
    {
        ///<summary>
        /// Gets a list of all the available network adapters on a system.
        ///</summary>
        public static List<NetworkAdapter> GetAdapters()
        {
            var adapters = new List<NetworkAdapter>();

            var strWQuery = "SELECT DeviceID, ProductName, Description, "
                            + "NetEnabled, NetConnectionStatus "
                            + "FROM Win32_NetworkAdapter "
                            + "WHERE Manufacturer <> 'Microsoft' ";
            var oQuery = new System.Management.ObjectQuery(strWQuery);
            var oSearcher = new ManagementObjectSearcher(oQuery);
            var oReturnCollection = oSearcher.Get();

            foreach (ManagementObject mo in oReturnCollection)
            {
                try
                {
                    if (mo == null)
                        continue;

                    var devId = Convert.ToInt32(mo["DeviceID"].ToString());
                    var productName = mo["ProductName"].ToString();
                    var description = mo["Description"].ToString();
                    var netConnectionStatus = mo["NetConnectionStatus"] == null
                        ? -1
                        : Convert.ToInt32(mo["NetConnectionStatus"].ToString());

                    var netEnabled = Convert.ToBoolean(mo["NetEnabled"].ToString());

                    adapters.Add(new NetworkAdapter(
                        devId,
                        productName,
                        description,
                        netEnabled,
                        netConnectionStatus));
                }
                catch (NullReferenceException)
                {
                    // ignore some adapters, such as the Bluetooth adapter, that need user interaction
                    // to enable/disable, per <https://stackoverflow.com/questions/49685601/how-to-close-all-connections-to-internet>

                }
            }

            return adapters;
        }
    }
}