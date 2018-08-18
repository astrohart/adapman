using System;

namespace adapman
{
    public static class Program
    {
        private static CommandLineInfo _theCommandLine;

        public static CommandLineInfo CommandLine => _theCommandLine ??
                       (_theCommandLine = CommandLineInfo.ParseCommandLine(Environment.GetCommandLineArgs()));

        public static void Main(string[] args)
        {
            if (CommandLine == null)
            {
                CommandLineInfo.PrintUsageMessage();
                return;
            }

            switch (CommandLine.Action)
            {
                case CommandLineAction.DisableAllAdapters:
                    foreach (var adapter in NetworkAdapterManager.GetAdapters())
                    {
                        adapter.Disable();
                    }
                    break;
                
                case CommandLineAction.EnableAllAdapters:
                    foreach (var adapter in NetworkAdapterManager.GetAdapters())
                    {
                        adapter.Enable();
                    }
                    break;
                
                case CommandLineAction.ConnectWifi:
                    WifiManager.Connect(CommandLine.WifiSSID, CommandLine.WifiPassword);
                    break;

                case CommandLineAction.DisconnectWifi:
                    WifiManager.Disconnect(CommandLine.WifiSSID);
                    break;
                
                case CommandLineAction.Unknown:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}