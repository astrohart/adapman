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
        /// <summary>
        /// Enables the <see cref="T:adapman.NetworkAdapter" /> referenced by the
        /// <see cref="adapter" /> (sets it as being able to connect to
        /// networks).
        /// </summary>
        /// <param name="adapter">
        /// Reference to an instance of <see cref="T:adapman.NetworkAdapter" />
        /// that represents the particular adapter you want to enable.
        /// </param>
        public static void Enable(NetworkAdapter adapter)
        {
            // adapter paramter must not be null
            if (adapter == null)
                return;

            // adapter DeviceId must be a positive integer
            if (adapter.DeviceID <= 0)
                return;

            try
            {
                var currentManagementObject = new ManagementObject();
                var queryString =
                    "SELECT DeviceID,ProductName,Description,NetEnabled FROM " +
                    $"Win32_NetworkAdapter WHERE DeviceID = {adapter.DeviceID}";

                var query = new ObjectQuery(queryString);
                var searcher = new ManagementObjectSearcher(query);
                var results = searcher.Get();

                foreach (var managementBaseObject in results)
                {
                    currentManagementObject = (ManagementObject)managementBaseObject;
                }

                // Enable the Network Adapter by invoking the appropriate method
                // on the WMI management object found by the query above
                currentManagementObject?.InvokeMethod("Enable", null);

                adapter.IsEnabled = true;
            }
            catch (Exception ex)
            {
                // Nothing here
                adapter.IsEnabled = false;      // failed to enable
            }
        }

        ///<summary>
        /// Gets a list of all the available network adapters on a system.
        ///</summary>
        /// <remarks>Returns the empty list if no network adapters could
        /// be found or if the list of network adapters could not be
        /// enumerated.</remarks>
        public static List<NetworkAdapter> GetAdapters()
        {
            var adapters = new List<NetworkAdapter>();

            // Windows Management Instrumentation (WMI) database query
            const string queryString = "SELECT DeviceID, ProductName, Description, "
                                       + "NetEnabled, NetConnectionStatus "
                                       + "FROM Win32_NetworkAdapter "
                                       + "WHERE Manufacturer <> 'Microsoft' ";

            var query = new ObjectQuery(queryString);
            var searcher = new ManagementObjectSearcher(query);
            var results = searcher.Get();

            // Check whether any results were obtained from our search. If we
            // have no results, then return the empty list (the default value of
            // the adapters variable)
            if (results.Count == 0)
                return adapters;

            foreach (var managementBaseObject in results)
            {
                var currentManagementObject = (ManagementObject)managementBaseObject;
                if (currentManagementObject == null)
                    continue;

                try
                {
                    // See the DataHelpers class for implementation of the
                    // GetValueWithDefault extension method
                    var devId = currentManagementObject["DeviceID"].GetValueWithDefault(-1);
                    var productName = currentManagementObject["ProductName"].GetValueWithDefault(String.Empty);
                    var description = currentManagementObject["Description"].GetValueWithDefault(String.Empty);
                    var netEnabled = currentManagementObject["NetEnabled"].GetValueWithDefault(false);
                    var netConnectionStatus = currentManagementObject["NetConnectionStatus"].GetValueWithDefault(-1);

                    // Initialize a new NetworkAdapter object with the values
                    // read above and then add a referenec to the
                    // newly-initalized object to the adapters list
                    var newAdapter = new NetworkAdapter(
                        devId,
                        productName,
                        description,
                        netEnabled,
                        netConnectionStatus);
                    adapters.Add(newAdapter);
                }
                catch (NullReferenceException)
                {
                    // ignore some adapters, such as the Bluetooth adapter, that
                    // need user interaction to enable/disable, per
                    // <https://stackoverflow.com/questions/49685601/how-to-close-all-connections-to-internet>
                    // The thinking is that a NullReferenceException is thrown
                    // when the code tries to query WMI for a Bluetooth adapter,
                    // so by ignoring NullReferenceExceptions, we are, in
                    // essence, ignoring any query results for a Bluetooth
                    // adapter.
                }
            }

            return adapters;
        }

        /// <summary>
        /// Disables the <see cref="T:adapman.NetworkAdapter" /> referenced by the
        /// <see cref="adapter" /> (sets it as not being able to connect to
        /// networks).
        /// </summary>
        /// <param name="adapter">
        /// Reference to an instance of <see cref="T:adapman.NetworkAdapter" />
        /// that represents the particular adapter you want to disable.
        /// </param>
        public static void Disable(NetworkAdapter adapter)
        {
            // adapter paramter must not be a null reference
            if (adapter == null)
                return;

            // adapter DeviceId must be a positive integer
            if (adapter.DeviceID <= 0)
                return;

            try
            {
                var currentManagementObject = new ManagementObject();
                var queryString =
                    "SELECT DeviceID,ProductName,Description,NetEnabled FROM " +
                    $"Win32_NetworkAdapter WHERE DeviceID = {adapter.DeviceID}";

                var query = new ObjectQuery(queryString);
                var searcher = new ManagementObjectSearcher(query);
                var results = searcher.Get();

                foreach (var managementBaseObject in results)
                {
                    currentManagementObject = (ManagementObject)managementBaseObject;
                }

                // Disable the Network Adapter by invoking the appropriate method
                // on the management object found
                currentManagementObject?.InvokeMethod("Disable", null);

                adapter.IsEnabled = false;
            }
            catch
            {
                // Nothing here
                adapter.IsEnabled = true;       // failed to disable
            }
        }
    }
}