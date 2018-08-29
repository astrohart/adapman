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
        /// <param name="args">Collection of strings that contains the arguments passed by the user on the command-line.</param>
        /// <returns>True if the command-line parameters are valid; false otherwise.</returns>
        public static bool IsCommandLineValid(string[] args)
        {
            // If args is a null reference, or it does not contain any elements
            // starting with the '-' character (for this program's switches),
            // then the command-line is invalid, and we return false.
            if (args == null || !args.Any(s => s.StartsWith("-")))
                return false;

            // The user must pass at least one switch on the command line.
            // Sometimes, the user passes two elements, a switch and a value.
            // The only other valid possibility is that the user passed two
            // switches, each with corresponding values
            switch (args.Length)
            {
                // The command-line is valid if the single element
                // of the args array consists of either the '-da'
                // or the '-ea' switches.  Case does not matter.
                case 1:
                    return args.Contains("-da") || args.Contains("-ea");

                case 2:
                    return args.First().StartsWith("-dw");

                case 4:
                    return args[0].StartsWith("-cw")
                     && !string.IsNullOrWhiteSpace(args[1])
                     && args[2].StartsWith("-cw")
                     || !string.IsNullOrWhiteSpace(args[3]);
            }

            return false;
        }
    }
}