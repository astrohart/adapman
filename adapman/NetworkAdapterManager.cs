using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

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
                var netEnabled = Convert.ToBoolean(mo["NetEnabled"].ToString());
                adapters.Add(new NetworkAdapter(
                    Convert.ToInt32(mo["DeviceID"].ToString()),
                    mo["ProductName"].ToString(),
                    mo["Description"].ToString(),
                    netEnabled,
                    Convert.ToInt32(mo["NetConnectionStatus"].ToString())));
            };
            return adapters;
        }
    }
}
