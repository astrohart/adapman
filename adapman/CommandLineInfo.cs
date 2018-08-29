using adapman.Properties;
using System;
using System.Linq;

namespace adapman
{
    /// <summary>
    /// Provides functionality to expose settings based on the command-line
    /// options provided to a program by the user.
    /// </summary>
    public class CommandLineInfo
    {
        ///<summary>
        /// Constructs a new instance of <see cref="T:adapman.CommandLineInfo"/>.
        ///</summary>
        /// <param name="action">One of the <see cref="T:adapman.CommandLineAction"/>
        /// values that specifies what the user wants to do.</param>
        /// <param name="wifiSSID">(Optional.) Specifies the SSID of the Wi-Fi network
        /// that the specified action applies to.</param>
        /// <param name="wifiPassword">(Optional.) Specifies the password, or network
        /// security key, of the Wi-Fi network that the specified action applies to.</param>
        protected CommandLineInfo(CommandLineAction action, string wifiSSID = "", string wifiPassword = "")
        {
            // THe action will always be specified
            Action = action;

            // If the action does not apply to a specific Wi-Fi network, the
            // properties below will be initialized to blank by default.
            WifiSSID = wifiSSID;
            WifiPassword = wifiPassword;
        }

        /// <summary>
        /// Gets a value indicating the action that the user wants the program to
        /// take. One of the <see cref="T:adapman.CommandLineAction" /> values.
        /// </summary>
        public CommandLineAction Action { get; }

        /// <summary>
        /// Gets a string containing the password to the Wi-Fi network that the
        /// user wants us to connect to.
        /// </summary>
        public string WifiPassword { get; }

        /// <summary>
        /// Gets a string containing the SSID of the Wi-Fi network that the user
        /// wants to work with.
        /// </summary>
        public string WifiSSID { get; }

        /// <summary>
        /// Parses the command-line arguments and returns a reference to an
        /// instance of <see cref="T:adapman.CommandLineInfo" /> whose properties
        /// are filled with values initialized by the options the user chose.
        /// </summary>
        /// <param name="args">
        /// Reference to an array of strings that contains the command-line arguments passed to this program.
        /// </param>
        /// <returns>
        /// Reference to an instance of <see cref="T:adapman.CommandLineInfo"/> whose properties
        /// are filled with the values initialized by the options the user chose; if the user did not
        /// pass valid command-line arguments, or the command-line could otherwise not be parsed,
        /// a usage mesage is printed to the screen and this method returns a null reference.
        /// </returns>
        public static CommandLineInfo ParseCommandLine(string[] args)
        {
            // Skip the first element of args if it contains the application
            // executable's filename since it's usually initialized by
            // Environment.GetCommandLineArgs which passes the full path to this
            // executeable as the first element of args
            if (args.Length >= 1 && 
                !string.IsNullOrWhiteSpace(args[0]) 
                && args[0].EndsWith("exe"))   // strip out the first elt of args, if it ends with "exe"
                args = args.Skip(1).ToArray();

            // If we get here, then more arguments exist than simply the path to the executeable itself.
            // In this case, run the CommandLineValidator to ensure the arguments passed are valid.
            // (See CommandLineValidator.cs)
            if (!CommandLineValidator.IsCommandLineValid(args))
            {
                // If the command-line validator determined that the arguments passed are not valid, 
                // print a usage message to the console, and then return the value null instead of
                // a reference to an instance of CommandLineInfo, so as to signal the caller that the 
                // command-line could not be parsed.
                PrintUsageMessage();
                return null;
            }

            // Getting ready to return the result
            CommandLineInfo result = null;

            // Determine what action should be taken based on the value of the first command-line
            // switch, since the user is required to pass at least one switch to this program.

            var ssid = string.Empty;
            var password = string.Empty;

            switch (args[0])
            {
                // This switch disables all the network adapters on the user's computer.
                case "-da":
                    // New up a new CommandLineInfo instance and pass it an action of DisableAllAdapters
                    result = new CommandLineInfo(CommandLineAction.DisableAllAdapters);
                    break;

                // This switch enables all the network adapters on the user's computer.
                case "-ea":
                    // New up a new CommandLineInfo instance and pass it an action of EnableAllAdapters
                    result = new CommandLineInfo(CommandLineAction.EnableAllAdapters);
                    break;

                // The '-cw:ssid' switch, which must be paired with the '-cw:pwd' switch,
                // connects the user's computer to the Wi-Fi network whose SSID and password
                // (i.e., network security key) follow the corresponding switches.
                case "-cw:ssid" when args[2] == "-cw:pwd":
                    // New up a new CommandLineInfo instance and pass it the action of ConnectWifi.  We
                    // expect that the SSID and password (i.e., network security key) of the network to be
                    // connected should be in the second and fourth command-line parameters passed (following
                    // the corresponding switches)
                    
                    // Obtain the SSID from the arguments, and be sure to bounds-check
                    ssid = string.Empty;
                    if (args.Length >= 2)
                        ssid = args[1];

                    // Obtain the password (i.e., network security key), and be sure to bounds-check
                    if (args.Length >= 4)
                        password = args[3];
                    
                    // Create the resulting instance of CommandLineInfo, initialized with the
                    // SSID and network security key passed by the user on the command line.
                    result = new CommandLineInfo(CommandLineAction.ConnectWifi, 
                        ssid, password); // values following each flag are inputs
                    break;

                // The '-dw:ssid' switch disconnects the user's computer from the Wi-Fi network with 
                // the specified ID
                case "-dw:ssid":
                    // Obtain the SSID from the arguments, and be sure to bounds-check
                    ssid = string.Empty;
                    if (args.Length >= 2)
                        ssid = args[1];

                    // Create a resulting instance of CommandLineInfo that is initialized with 
                    // the Action DisconnectWifi, and the SSID of the network to disconnect from
                    result = new CommandLineInfo(CommandLineAction.DisconnectWifi, ssid);    // value following the flag is an input
                    break;
            }

            // return the instance of the either properly-filled out CommandLineInfo object, 
            // or the null reference if the command-line the user passed to this program
            // turned out to be invalid or unparsable.
            return result;
        }

        /// <summary>
        /// Prints a usage message to the console, and then exits the process.
        /// This is usually called in the case where the command-line arguments
        /// are not valid.
        /// </summary>
        /// <remarks>If this usage message comes from a direct launch, by the </remarks>
        public static void PrintUsageMessage()
        {
            // Retrieve the usage message from the resources and print it
            // to the console
            Console.WriteLine(Resources.UsageMessage);

            // Force the program to terminate -- but only if we are not, 
            // e.g., running this code in a unit testing environment.
            // We can tell whether this is the case, by examining the first
            // element of the array returned by Environment.GetCommandLineArgs().

            var firstArg = Environment.GetCommandLineArgs().First();
            if (!firstArg.Contains("adapman"))
            {
                // However, most unit-test environments use a different executable to 
                // launch dynamically-compiled code, and to avoid making the IDE or
                // other unit-testing environment crash, we simply return from this
                // method in the case that the first element returned by the 
                // Environment.GetCommandLineArgs() method is not the same as the name
                // of the executable.
                return;
            }

            // If the first element of the array contains the word 'adapman',
            // which is the name of this executable, then we know that we are running
            // because the user launched adapman.exe, and we should forcibly terminate
            // the running software after the usage message has been printed.
            Environment.Exit(-1);
        }
    }
}