using System.Management;

namespace adapman
{
    /// <summary>
    /// Provides an encapsulation of a network adapter entry in Windows.
    /// </summary>
    public class NetworkAdapter
    {
        /// <summary>
        /// Constructs a new instance of <see cref="T:adapman.NetworkAdapter"/>
        /// and returns a refernce to it.
        /// </summary>
        /// <param name="deviceId">Integer specifying the Windows device ID of the adapter.</param>
        /// <param name="productName">String specifying the product name of the adapter.</param>
        /// <param name="description">String containing the description listed
        /// in the system for the adapter.</param>
        /// <param name="enabled">Flag indicating whether this adapter is enabled.</param>
        /// <param name="connStatus">Integer value indicating the connected state of the adapter.</param>
        public NetworkAdapter(int deviceId, string productName, 
            string description, bool enabled, int connStatus)
        {
            DeviceID = deviceId;
            ProductName = productName;
            Description = description;
            IsEnabled = enabled;
            ConnStatus = connStatus;
        }

        /// <summary>
        /// Gets an integer value that specifies the connected state of this adapter.
        /// </summary>
        public int ConnStatus { get; }

        /// <summary>
        /// Gets a string containing the description of this network adapter as listed in the system.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets an integer value specifying the Windows device ID of this adapter.
        /// </summary>
        public int DeviceID { get; }

        /// <summary>
        /// Gets a flag indicating whether this adapter is enabled.
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// Gets a string containing the product name of this adapter as listed by the operating system.
        /// </summary>
        public string ProductName { get; }

        /// <summary>
        /// Disables this network adapter (i.e., makes it unusable for connecting to networks).
        /// </summary>
        public void Disable()
        {
            DoDisable();
        }

        /// <summary>
        /// Enables this network adapter (sets it as being able to connect to networks).
        /// </summary>
        public void Enable()
        {
            DoEnable();
        }

        /// <summary>
        /// Implements the code to enable a network adapter.
        /// </summary>
        private void DoEnable()
        {
            try
            {
                var currentManagementObject = new ManagementObject();
                var queryString =
                    $"SELECT DeviceID,ProductName,Description,NetEnabled FROM Win32_NetworkAdapter WHERE DeviceID = {DeviceID}";

                var query = new ObjectQuery(queryString);
                var searcher = new ManagementObjectSearcher(query);
                var results = searcher.Get();

                foreach (var managementBaseObject in results)
                {
                    currentManagementObject = (ManagementObject) managementBaseObject;
                }

                //Enable the Network Adapter by invoking the appropriate method on the 
                // management object found
                currentManagementObject.InvokeMethod("Enable", null);

                IsEnabled = true;
            }
            catch
            {
                // Nothing here
                IsEnabled = false;      // failed to enable
            }
        }

        private void DoDisable()
        {
            try
            {
                var currentMObject = new ManagementObject();
                var strWQuery =
                    $"SELECT DeviceID,ProductName,Description,NetEnabled FROM Win32_NetworkAdapter WHERE DeviceID = {DeviceID}";

                var oQuery = new ObjectQuery(strWQuery);
                var oSearcher = new ManagementObjectSearcher(oQuery);
                var oReturnCollection = oSearcher.Get();

                foreach (var mo in oReturnCollection)
                {
                    currentMObject = (ManagementObject) mo;
                }

                //Disable the Network Adapter
                currentMObject.InvokeMethod("Disable", null);

                IsEnabled = false;
            }
            catch
            {
                // Nothing here
                IsEnabled = true;       // failed to disable
            }
        }
    }
}