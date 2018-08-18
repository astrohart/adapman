using System.Linq;

namespace adapman
{
    /// <summary>
    /// Contains methods to validate command-line argument combinations.
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
            if (args == null || !args.Any(s => s.StartsWith("-")))
                return false;

            switch (args.Length)
            {
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