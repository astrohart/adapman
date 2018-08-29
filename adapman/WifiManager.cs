using adapman.Properties;
using NativeWifi;
using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace adapman
{
    public static class WifiManager
    {
        public static bool IsConnected
        {
            get
            {
                var client = new WlanClient();
                if (!client.Interfaces.Any())
                    return false;

                var wlanInterface = client.Interfaces.FirstOrDefault();
                if (wlanInterface == null)
                    return false;
                return wlanInterface.InterfaceState == Wlan.WlanInterfaceState.Connected;
            }
        }

        public static void Connect(string ssid, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ssid))
                    throw new ArgumentNullException(nameof(ssid), Resources.ConnectSSIDReq);

                var client = new WlanClient();
                var wlanInterface = client.Interfaces.FirstOrDefault();
                if (wlanInterface == null)
                    return;

                var mac = StringToHex(ssid);
                var key = password;
                var profile = string.Format(
                    new XDocument(new XElement(
                        XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "WLANProfile",
                        new XElement(XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "name",
                            "{0}"),
                        new XElement(
                            XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "SSIDConfig",
                            new XElement(XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "SSID",
                                new XElement(
                                    XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "hex",
                                    "{1}"),
                                new XElement(
                                    XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "name",
                                    "{0}"))),
                        new XElement(
                            XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "connectionType",
                            "ESS"),
                        new XElement(
                            XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "connectionMode",
                            "auto"),
                        new XElement(XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "MSM",
                            new XElement(
                                XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "security",
                                new XElement(
                                    XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") +
                                    "authEncryption",
                                    new XElement(
                                        XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") +
                                        "authentication", "WPA2PSK"),
                                    new XElement(
                                        XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") +
                                        "encryption", "AES"),
                                    new XElement(
                                        XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") +
                                        "useOneX",
                                        "false")),
                                new XElement(
                                    XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "sharedKey",
                                    new XElement(
                                        XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") +
                                        "keyType",
                                        "passPhrase"),
                                    new XElement(
                                        XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") +
                                        "protected",
                                        "false"),
                                    new XElement(
                                        XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") +
                                        "keyMaterial", "{2}")))))).ToString(), ssid, mac, key);

                wlanInterface.SetProfile(Wlan.WlanProfileFlags.AllUser, profile, true);
                wlanInterface.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, ssid);
            }
            catch
            {
                // This code works irregardless of whether any exceptions are
                // raised or not.
            }
        }

        public static void Disconnect(string ssid)
        {
            if (string.IsNullOrWhiteSpace(ssid))
                throw new ArgumentNullException(nameof(ssid), Resources.DiisconnectSSIDReq);

            var client = new WlanClient();

            var wlanInterface = client.Interfaces.FirstOrDefault();

            wlanInterface?.DeleteProfile(ssid);
        }

        private static string GetStringForSSID(Wlan.Dot11Ssid ssid)
        {
            return Encoding.ASCII.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);
        }

        /// String to Hex
        private static string StringToHex(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value), Resources.ValueParamReq);

            var sb = new StringBuilder();
            var bytes = Encoding.Default.GetBytes(value);
            foreach (var t in bytes)
            {
                sb.Append(Convert.ToString(t, 16));
            }
            return (sb.ToString().ToUpper());
        }
    }
}