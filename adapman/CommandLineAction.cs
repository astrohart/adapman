namespace adapman
{
    /// <summary>
    /// Describes the action the user wants this application to take,
    /// based on which switches have been passed on the command-line.
    /// </summary>
    public enum CommandLineAction
    {
        /// <summary>
        /// Disables all network adapters on the user's computer, totally disabling connectivity.
        /// </summary>
        DisableAllAdapters,

        /// <summary>
        /// Enables all network adapters on the user's computer, thereby making it possible for the user to potentially connect to a network.
        /// </summary>
        EnableAllAdapters,

        /// <summary>
        /// Connects the WiFi adapter (if present) to the network with the specified SSID and password (i.e., network security key).
        /// </summary>
        ConnectWifi,

        /// <summary>
        /// Disconnects the WiFi adapter (if present) from the network with the specified SSID.
        /// </summary>
        DisconnectWifi,

        /// <summary>
        /// Default, unknown value
        /// </summary>
        Unknown
    }
}