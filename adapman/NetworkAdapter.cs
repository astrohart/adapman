using System.Management;

namespace adapman
{
    public class NetworkAdapter
    {
        public NetworkAdapter(int deviceId, string productName, string description, bool enabled, int connStatus)
        {
            DeviceID = deviceId;
            ProductName = productName;
            Description = description;
            IsEnabled = enabled;
            ConnStatus = connStatus;
        }

        public int ConnStatus { get; private set; }
        public string Description { get; private set; }
        public int DeviceID { get; private set; }
        public bool IsEnabled { get; private set; }
        public string ProductName { get; private set; }

        public void Disable()
        {
            if (!IsEnabled)
                return;

            ToggleEnabled();
        }

        public void Enable()
        {
            if (IsEnabled)
                return;

            ToggleEnabled();
        }

        private void ToggleEnabled()
        {
            var currentMObject = new ManagementObject();
            var strWQuery =
                $"SELECT DeviceID,ProductName,Description,NetEnabled FROM Win32_NetworkAdapter WHERE DeviceID = {DeviceID}";

            var oQuery = new System.Management.ObjectQuery(strWQuery);
            var oSearcher = new ManagementObjectSearcher(oQuery);
            var oReturnCollection = oSearcher.Get();

            foreach (var mo in oReturnCollection)
            {
                currentMObject = (ManagementObject)mo;
            }

            //Enable the Network Adapter
            currentMObject.InvokeMethod(IsEnabled ? "Disable" : "Enable", null);

            IsEnabled = !IsEnabled;
        }
    }
}