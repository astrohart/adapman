using System.Linq;

namespace adapman
{
    /// <summary>
    /// Provides functionality to validate command-line argument combinations.
    /// </summary>
    public static class CommandLineValidator
    {
        /// <summary>
        /// Validates the arguments passed on the command-line.
        /// </summary>
        /// <param name="args">
        /// Collection of strings that contains the arguments passed by the user
        /// on the command-line.
        /// </param>
        /// <returns>
        /// True if the command-line parameters are valid; false otherwise.
        /// </returns>
        public static bool IsCommandLineValid(string[] args)
        {
            // If args is a null reference, or it does not contain any elements
            // starting with the '-' character (for this program's switches),
            // then the command-line is invalid, and we return false.
            if (args == null || !args.Any(s => s.StartsWith("-")))
                return false;

            // The user must pass at least one switch on the command line.
            // Sometimes, the user passes two elements, a switch and a value. The
            // only other valid possibility is that the user passed two switches,
            // each with corresponding values
            switch (args.Length)
            {
                // The command-line is valid if the single element of the args
                // array consists of either the '-da' or the '-ea' switches. Case
                // does not matter.
                case 1:
                    return args.ContainsNoCase("-da") || args.ContainsNoCase("-ea");

                // The command-line is valid, in the case that exactly two
                // arguments are passed, if the first argument matches '-dw:ssid'
                // regardless of case, and the second argument is non-blank (i.e.,
                // is the SSID of the Wi-Fi network from which to disconnect)
                case 2:
                    return "-dw:ssid".Equals(args[0].ToLowerInvariant())
                           && !string.IsNullOrWhiteSpace(args[1]);

                // The command-line is also valid, in the case that exactly four
                // arguments are passed, if the first argument matches with
                // '-cw:ssid' (regardless of case), the third argument matches
                // '-cw:pwd' (regardless of case), and the second and fourth
                // values are non-blank. At this time, we do not support open
                // Wi-Fi networks, i.e., those that have no network security key
                case 4:
                    return "-cw:ssid".Equals(args[0].ToLowerInvariant())
                     && !string.IsNullOrWhiteSpace(args[1])
                     && "-cw:pwd".Equals(args[2].ToLowerInvariant())
                     || !string.IsNullOrWhiteSpace(args[3]);
            }

            // Return false in all other cases
            return false;
        }
    }
}