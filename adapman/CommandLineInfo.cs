using System;
using System.Linq;
using adapman.Properties;

namespace adapman
{
    public class CommandLineInfo
    {
        ///<summary>
        /// Constructs a new instance of <see cref="T:adapman.CommandLineInfo"/>.
        ///</summary>
        /// <param name="action">One of the <see cref="T:adapman.CommandLineAction"/> values that specifies what the user wants to do.</param>
        /// <param name="wifiSSID">(Optional.) Specifies the SSID of the Wi-Fi network that the specified action applies to.</param>
        /// <param name="wifiPassword">(Optional.) Specifies the password, or network security key, of the Wi-Fi network that the specified action applies to.</param>
        protected CommandLineInfo(CommandLineAction action, string wifiSSID = "", string wifiPassword = "")
        {
            Action = action;
            WifiSSID = wifiSSID;
            WifiPassword = wifiPassword;
        }

        public CommandLineAction Action { get; }

        public string WifiPassword { get; }
        public string WifiSSID { get; }

        /// <summary>
        /// Parses the command-line arguments and returns a reference to an instance of <see cref="T:adapman.CommandLineInfo"/> whose
        /// properties are filled with values initialized by the options the user chose.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static CommandLineInfo ParseCommandLine(string[] args)
        {
            // Skip the first element of args if it contains the application executable's filename
            // since it's usually initialized by Environment.GetCommandLineArgs which passes the full
            // path to this executeable as the first element of args
            if (args[0].Contains("adapman"))    // strip out the first elt
                args = args.Skip(1).ToArray();

            if (!CommandLineValidator.IsCommandLineValid(args)) 
            {
                PrintUsageMessage();
                return null;
            }

            CommandLineInfo result = null;

            switch (args[0])
            {
                case "-da":
                    result = new CommandLineInfo(CommandLineAction.DisableAllAdapters);
                    break;

                case "-ea":
                    result = new CommandLineInfo(CommandLineAction.EnableAllAdapters);
                    break;

                case "-cw:ssid" when args[2] == "-cw:pwd":
                    result = new CommandLineInfo(CommandLineAction.ConnectWifi, args[1], args[3]); // values following each flag are inputs
                    break;

                case "-dw:ssid":
                    result = new CommandLineInfo(CommandLineAction.DisconnectWifi, args[1]);    // value following the flag is an input
                    break;
            }

            return result;
        }

        /// <summary>
        /// Prints a usage message to the console, and then exits the process.  This is usually called in the case
        /// where the command-line arguments are not valid.
        /// </summary>
        public static void PrintUsageMessage()
        {
            Console.WriteLine(Resources.UsageMessage);
            var firstArg = Environment.GetCommandLineArgs().First();
            if (!firstArg.Contains("adapman")) {
                return;
            }

            Environment.Exit(-1);
        }
    }
}