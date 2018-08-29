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

            var queryString = "SELECT DeviceID, ProductName, Description, "
                            + "NetEnabled, NetConnectionStatus "
                            + "FROM Win32_NetworkAdapter "
                            + "WHERE Manufacturer <> 'Microsoft' ";
            var query = new ObjectQuery(queryString);
            var searcher = new ManagementObjectSearcher(query);
            var results = searcher.Get();

            foreach (var managementBaseObject in results)
            {
                var currentManagementObject = (ManagementObject)managementBaseObject;
                try
                {
                    if (currentManagementObject == null)
                        continue;

                    var devId = Convert.ToInt32(currentManagementObject["DeviceID"].ToString());
                    var productName = currentManagementObject["ProductName"].ToString();
                    var description = currentManagementObject["Description"].ToString();
                    var netConnectionStatus = currentManagementObject["NetConnectionStatus"] == null
                        ? -1
                        : Convert.ToInt32(currentManagementObject["NetConnectionStatus"].ToString());

                    var netEnabled = Convert.ToBoolean(currentManagementObject["NetEnabled"].ToString());

                    adapters.Add(new NetworkAdapter(
                        devId,
                        productName,
                        description,
                        netEnabled,
                        netConnectionStatus));
                }
                catch (NullReferenceException)
                {
                    // ignore some adapters, such as the Bluetooth adapter, that
                    // need user interaction to enable/disable, per
                    // <https://stackoverflow.com/questions/49685601/how-to-close-all-connections-to-internet>
                }
            }

            return adapters;
        }
    }
}