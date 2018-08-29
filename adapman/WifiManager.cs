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
                    // Get a new instance of the NativeWifi.WlanClient class
                    var client = new WlanClient();

                    // To determine if the computer is connected to a Wi-Fi network, we
                    // simply ask if any of the computer's Wi-Fi interfaces are in the
                    // Connected state.
                    result = client.Interfaces.Any(iface =>
                        iface.InterfaceState == Wlan.WlanInterfaceState.Connected);
                }
                catch
                {
                    // If we are here, then some kind of operating system error must
                    // have occurred.  Ignore the error, and simply set the return value
                    // of this property to false, which is its default value.

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
                // ssid is a required parameter
                if (string.IsNullOrWhiteSpace(ssid))
                    throw new ArgumentNullException(nameof(ssid), Resources.ConnectSSIDReq);

                // password is a required parameter
                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentNullException(nameof(ssid), Resources.ConnectPasswordReq);

                // Allocate a new instance of the NativeWifi.WlanClient class and get a reference to
                // the first available Wi-Fi network interface.
                var client = new WlanClient();
                var wlanInterface = client.Interfaces.FirstOrDefault();
                if (wlanInterface == null)
                    return;

                // Obtain values to be utilized in the Wi-Fi profile XML below.
                var mac = StringToHex(ssid);
                var key = password;

                // Format the Wi-Fi profile XML
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

                // Set the current Wi-Fi interface to use the profile above, and then tell it to connect.
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

            // Allocate a new instance of the NativeWifi.WlanClient that manages
            // all this computer's Wi-Fi interfaces
            var client = new WlanClient();

            // Obtain a reference to the first available Wi-Fi interface.
            var wlanInterface = client.Interfaces.FirstOrDefault();
            if (wlanInterface == null)
                return; // failed to obtain a Wi-Fi interface reference.

            // Delete the profile corresponding to the SSID specified, from the
            // Wi-Fi interface.
            wlanInterface.DeleteProfile(ssid);
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
        /// <remarks>This method does not seem to be currently called; however, I
        /// forget where it had been used, so until I remember, I won't delete it
        /// or comment it out.
        /// </remarks>
        private static string GetStringForSSID(Wlan.Dot11Ssid ssid)
        {
            // Return the SSID property of the ssid structure, encoded
            // in ASCII
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
        /// work.  If this method fails, the return value is the empty string.
        /// </remarks>
        private static string StringToHex(string value)
        {
            var result = string.Empty;

            // The value parameter is required.
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value), Resources.ValueParamReq);

            // Use a string builder
            var sb = new StringBuilder();

            // Get all the bytes of the value parameter as an array and
            // iterate through it.  Check to ensure there are more than
            // zero elements in the array obtained, before proceeding.
            // If the array has zero elements, return this method's
            // default value, which is the empty string.
            var bytes = Encoding.Default.GetBytes(value);
            if (bytes.Length == 0)
                return result;

            try
            {
                // For each of the bytes in the bytes array,
                // get its hexadecimal representation as a string
                // and add it to our string builder.  Then, once we're
                // done adding bytes, convert the resulting string to
                // uppercase (by convention).

                foreach (var t in bytes)
                {
                    var currentHexNumber = Convert.ToString(t, 16);
                    sb.Append(currentHexNumber);
                }

                result = sb.ToString().ToUpper();
            }
            catch
            {
                // I do not know what possible exceptions might get
                // raised here, but just in case one does, return this
                // method's default value, which is the empty string.
                result = string.Empty;
            }

            // return the resulting string
            return result;
        }
    }
}