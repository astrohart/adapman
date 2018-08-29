using System;
using System.Collections.Generic;
using System.Management;

namespace adapman
{
    ///<summary>
    /// Provides methods and functionality for managing the network adapters
    /// on the user's computer. 
    ///</summary>
    public static class NetworkAdapterManager
    {
        ///<summary>
        /// Gets a list of all the available network adapters on a system.
        ///</summary>
        /// <remarks>Returns the empty list if no network adapters could
        /// be found or if the list of network adapters could not be
        /// enumerated.</remarks>
        public static List<NetworkAdapter> GetAdapters()
        {
            var adapters = new List<NetworkAdapter>();

            var strWQuery = "SELECT DeviceID, ProductName, Description, "
                            + "NetEnabled, NetConnectionStatus "
                            + "FROM Win32_NetworkAdapter "
                            + "WHERE Manufacturer <> 'Microsoft' ";
            var oQuery = new ObjectQuery(strWQuery);
            var oSearcher = new ManagementObjectSearcher(oQuery);
            var oReturnCollection = oSearcher.Get();

            foreach (var o in oReturnCollection)
            {
                var mo = (ManagementObject) o;
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