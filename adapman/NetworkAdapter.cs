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
        public string Description { get; }
        public int DeviceID { get; }
        public bool IsEnabled { get; private set; }
        public string ProductName { get; }

        public void Disable()
        {
            DoDisable();
        }

        public void Enable()
        {
            DoEnable();
        }

        private void DoEnable()
        {
            try
            {
                var currentMObject = new ManagementObject();
                var strWQuery =
                    $"SELECT DeviceID,ProductName,Description,NetEnabled FROM Win32_NetworkAdapter WHERE DeviceID = {DeviceID}";

                var oQuery = new System.Management.ObjectQuery(strWQuery);
                var oSearcher = new ManagementObjectSearcher(oQuery);
                var oReturnCollection = oSearcher.Get();

                foreach (var mo in oReturnCollection)
                {
                    currentMObject = (ManagementObject) mo;
                }

                //Enable the Network Adapter
                currentMObject.InvokeMethod("Enable", null);

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

                var oQuery = new System.Management.ObjectQuery(strWQuery);
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