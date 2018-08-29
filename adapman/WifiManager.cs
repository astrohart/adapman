using adapman.Properties;
using NativeWifi;
using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace adapman
{
    /// <summary>
    /// Provides methods and functionality for managing the computer's Wi-Fi
    /// connections.
    /// </summary>
    public static class WifiManager
    {
        /// <summary>
        /// Gets a value indicating whether this computer is currently connected
        /// to a Wi-Fi network.
        /// </summary>
        /// <remarks>
        /// This property returns true the first time it sees a Wi-Fi interface
        /// that is connected to a Wi-Fi network. If there are more than one
        /// Wi-Fi adapter in the user's computer, this property will think the
        /// computer is connected to Wi-Fi if just one of the adapter(s) or
        /// interface(s) is connected.
        /// </remarks>
        public static bool IsConnected
        {
            get
            {
                var result = false;

                try
                {
                    var client = new WlanClient();
                    result = client.Interfaces.Any(iface =>
                        iface.InterfaceState == Wlan.WlanInterfaceState.Connected);
                }
                catch
                {
                    result = false;
                }

                return result;
            }
        }

        /// <summary>
        /// Connects the user's computer to the Wi-Fi network with the specified
        /// <see cref="ssid" /> and <see cref="password" /> (i.e., network
        /// security key). Will not work for so-called "open" Wi-Fi hotspots.
        /// </summary>
        /// <param name="ssid">
        /// (Required.) SSID of the Wi-Fi network to which the user wants to
        /// connect.
        /// </param>
        /// <param name="password">
        /// (Required.) The password, i.e., network security key, of the Wi-Fi
        /// network to which the user wants to connect.
        /// </param>
        /// <remarks>
        /// Attempts to connect to the Wi-Fi network with SSID and network
        /// security key specified by the <see cref="ssid" /> and
        /// <see cref="password" /> parameters. Uses the first available Wi-Fi
        /// interface on a system.
        /// </remarks>
        /// <exception cref="T:System.ArgumentNullException">
        /// Thrown if the <see cref="ssid" /> value is a blank string.
        /// </exception>
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

        /// <summary>
        /// Attempts to disconnect the user's Wi-Fi adapter from the network with
        /// the SSID specified.
        /// </summary>
        /// <param name="ssid">
        /// (Required.) SSID of the Wi-Fi network from which the user wants to
        /// disconnect.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// Thrown if the <see cref="ssid" /> value is a blank string.
        /// </exception>
        public static void Disconnect(string ssid)
        {
            if (string.IsNullOrWhiteSpace(ssid))
                throw new ArgumentNullException(nameof(ssid), Resources.DiisconnectSSIDReq);

            var client = new WlanClient();

            var wlanInterface = client.Interfaces.FirstOrDefault();

            wlanInterface?.DeleteProfile(ssid);
        }

        /// <summary>
        /// Gets the string representation of a given Wi-Fi network's SSID from
        /// the reference to the instance of
        /// <see cref="T:NativeWifi.Wlan.Dot11Ssid" /> specified in the
        /// <see cref="ssid" /> parameter.
        /// </summary>
        /// <param name="ssid">
        /// Reference to an instance of
        /// <see cref="T:NativeWifi.Wlan.Dot11Ssid" /> representing the SSID of
        /// the desired Wi-Fi network.
        /// </param>
        /// <returns>
        /// String representation of the SSID of the Wi-Fi network referenced by
        /// the <see cref="ssid" /> parameter.
        /// </returns>
        private static string GetStringForSSID(Wlan.Dot11Ssid ssid)
        {
            return Encoding.ASCII.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);
        }

        /// <summary>
        /// Converts a string of characters to the hexadecimal equivalent.
        /// </summary>
        /// <param name="value">
        /// (Required.) String value to be converted.
        /// </param>
        /// <returns>
        /// String containing the hexadecimal equivalent of the string's
        /// characters, in the default encoding of the string.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// Thrown if the <see cref="value" /> parameter is blank.
        /// </exception>
        /// <remarks>
        /// This method is required for the <see cref="Connect" /> method to
        /// work.
        /// </remarks>
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