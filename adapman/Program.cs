using System;
using adapman.Properties;

namespace adapman
{
    /// <summary>
    /// Provides methods and functionality for the main program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Holds a reference to an instance of <see cref="T:adapman.CommandLineInfo"/> that contains values that
        /// correspond to the options the user selected by passing certain command-line parameters.
        /// </summary>
        private static CommandLineInfo _theCommandLine;

        /// <summary>
        /// Singleton that provides access to the (ostensibly) one and only instance of <see cref="T:adapman.CommandLineInfo"/>
        /// whose properties contain values that correspond to the options the user has selected by passing certain command-line
        /// parameters.
        /// </summary>
        public static CommandLineInfo CommandLine => _theCommandLine ??
                       (_theCommandLine = CommandLineInfo.ParseCommandLine(Environment.GetCommandLineArgs()));

        /// <summary>
        /// Entry point for the application.
        /// </summary>
        /// <param name="args">List of string arguments passed by the user on the command line.</param>
        public static void Main(string[] args)
        {
            // Set up an exception handler for any uncaught exceptions
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            // Check whether the CommandLine property has a value of null.  Merely accessing this property should trigger
            // a call to the CommandLineInfo.ParseCommandLine method, which in turn returns either a reference to an instance
            // of a CommandLineInfo structure whose properties contain values corresponding to the options specified
            // by the command-line flags by the user, or, if the command line is empty or invalid, a null reference
            // is returned.
            if (CommandLine == null)
            {
                // If we are here, print the usage message to remind the user how to specify command line options
                // to this program, and then exit.
                CommandLineInfo.PrintUsageMessage();
                return;
            }

            // The (valid) command-line arguments to this program always specify an action to be taken, which is one of the 
            // values of the CommandLineAction enumeration.  Switch on this enumeration's value in order to determine what the 
            // user would like us to do.
            switch (CommandLine.Action)
            {
                // If we arrive here, then the user wants us to disable (i.e., turn off) all the network
                // adapters on this computer.
                case CommandLineAction.DisableAllAdapters:
                    // Get a list of all the network adapters on this computer, encapsulated by NetworkAdapter objects
                    // (see NetworkAdapterManager.cs and NetworkAdapter.cs) and then, for each adapter, call its Disable
                    // method to disable it (i.e., turn it off.).
                    foreach (var adapter in NetworkAdapterManager.GetAdapters()) {
                        // Assume the 'adapter' variable holds a non-null reference.
                        NetworkAdapterManager.Disable(adapter);
                    }
                    break;
                
                // If we arrive here, then th euser wants us to enable (i.e., turn on) all the network 
                // adapters on this computer.
                case CommandLineAction.EnableAllAdapters:
                    // Get a list of all the network adapters on this computer, encapsulated by NetworkAdapter objects
                    // (see NetworkAdapterManager.cs and NetworkAdapter.cs) and then, for each adapter, call its Enable
                    // method to enable it (i.e., turn it on).
                    foreach (var adapter in NetworkAdapterManager.GetAdapters()) {
                        // Assume the 'adapter' variable holds a non-null reference.
                        NetworkAdapterManager.Enable(adapter);
                    }
                    break;
                
                // If we arrive here, the user wants to connect to a Wi-Fi network with the specified SSID, and,
                // optionally, with the specified password
                case CommandLineAction.ConnectWifi:
                    // Call the Connect method of WifiManager to do this operation -- see WifiManager.cs.
                    WifiManager.Connect(CommandLine.WifiSSID, CommandLine.WifiPassword);
                    break;

                // If we arrive here, the user wants to disconnect from the Wi-Fi network with the specified SSID.
                case CommandLineAction.DisconnectWifi:
                    // Call the Disconnect method of WifiManager to do this operation -- see WifiManager.cs.  Test
                    // to ensure that the user's Wi-Fi adapter is not already in the disconnected state.  If that is
                    // so, then there is no need to do a call to the WifiManager.Disconnect method.
                    if (!WifiManager.IsConnected) {
                        // Write a message to the screen informing the user that the Wi-Fi adapter is already 
                        // in the disconnected state.
                        Console.WriteLine(Resources.WifiAdapterAlreadyDisconnected);
                        break;
                    }

                    // Disconnect from the Wi-Fi network that has the specified SSID.
                    WifiManager.Disconnect(CommandLine.WifiSSID);
                    break;
                
                // This handles the case where the command-line parser somehow validates wrong command-line arguments, and, 
                // as a result, does not pass null to the Main method but triggers this switch/case -- however, if we are here
                // then this means that the user passed something on the command-line that we do not know what action it corresponds
                // to.
                case CommandLineAction.Unknown:
                    break;

                // If we arrive here, then an invalid value was passed in the CommandLineInfo.Action property.
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Done
        }

        /// <summary>
        /// Handles the <see cref="T:System.AppDomain.UnhandledException"/> event.  This event is raised when code throws an
        /// exception for which there is no try/catch/finally block set up.
        /// </summary>
        /// <param name="sender">Reference to an instance of the object that raised this event.</param>
        /// <param name="e">Reference to an instance of <see cref="T:System.UnhandledExceptionEventArgs"/> that contains the event data.</param>
        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // To handle uncaught exceptions gracefully, we will simply write an error message to the console, 
            // which includes the exception's message text.  Be careful, since the e.ExceptionObject is of type
            // object, who knows?  It might not necessarily be cast-able to System.Exception.  So we pass it to a
            // helper method.
            var message = ExceptionHelpers.GetMessageFromExceptionObject(e.ExceptionObject);
            
            // Once we have obtained the message, write it to the console.  The UnhandledExceptionError resource string
            // is a formatted value and it can handle an empty message variable.
            Console.WriteLine(Resources.UnandledExceptionError, message);
        }
    }
}