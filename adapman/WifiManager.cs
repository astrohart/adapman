using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using adapman.Properties;
using NativeWifi;

namespace adapman
{
    public static class WifiManager
    {
        public static void Connect(string ssid, string password)
        {
            if (string.IsNullOrWhiteSpace(ssid))
                throw new ArgumentNullException(nameof(ssid), Resources.ConnectSSIDReq);

            var client = new WlanClient();
            var item = client.Interfaces.FirstOrDefault();
            if (item == null)
                return;

            var mac = StringToHex(ssid);
            var key = password;
            var profile = string.Format(
                new XDocument(new XElement(
                    XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "WLANProfile",
                    new XElement(XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "name", "{0}"),
                    new XElement(XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "SSIDConfig",
                        new XElement(XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "SSID",
                            new XElement(XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "hex",
                                "{1}"),
                            new XElement(XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "name",
                                "{0}"))),
                    new XElement(
                        XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "connectionType",
                        "ESS"),
                    new XElement(
                        XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "connectionMode",
                        "auto"),
                    new XElement(XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "MSM",
                        new XElement(XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "security",
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
                                    XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "useOneX",
                                    "false")),
                            new XElement(
                                XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "sharedKey",
                                new XElement(
                                    XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "keyType",
                                    "passPhrase"),
                                new XElement(
                                    XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") + "protected",
                                    "false"),
                                new XElement(
                                    XNamespace.Get("http://www.microsoft.com/networking/WLAN/profile/v1") +
                                    "keyMaterial", "{2}")))))).ToString(), ssid, mac, key);

            item.SetProfile(Wlan.WlanProfileFlags.AllUser, profile, true);
            item.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, ssid);
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