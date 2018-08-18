using NUnit.Framework;

namespace adapman.Tests
{
    [TestFixture]
    public class CommandLineInfoTests
    {
        [SetUp]
        public void Init()
        {

        }

        [Test]
        public void TestParseCommandLine1()
        {
            var cli = CommandLineInfo.ParseCommandLine(new[] {"-da"});

            Assert.IsTrue(cli.Action == CommandLineAction.DisableAllAdapters);
        }

        [Test]
        public void TestParseCommandLine2()
        {
            var cli = CommandLineInfo.ParseCommandLine(new[] {"-ea"});

            Assert.IsTrue(cli.Action == CommandLineAction.EnableAllAdapters);
        }

        [Test]
        public void TestParseCommandLine3()
        {
            Assert.IsFalse(CommandLineValidator.IsCommandLineValid(new[] {"asdlkfjlasd;kfjs;lakdfj"}));
        }

        [Test]
        public void TestParseCommandLine4()
        {
            Assert.IsFalse(CommandLineValidator.IsCommandLineValid(new[] {"-ea", "-ca"}));
        }

        [Test]
        public void TestParseCommandLine5()
        {
            Assert.IsFalse(CommandLineValidator.IsCommandLineValid(new[] {"-cw"}));
        }

        [Test]
        public void TestParseCommandLine6()
        {
            Assert.IsFalse(CommandLineValidator.IsCommandLineValid(new[] {"-cw:ssid", "-cw:pwd"}));
        }

        [Test]
        public void TestParseCommandLine7()
        {
            Assert.IsFalse(CommandLineValidator.IsCommandLineValid(new[] {"osjfjs", "lskfjjf", "sfjdskdf"}));
        }

        [Test]
        public void TestParseCommandLine8()
        {
            Assert.IsFalse(CommandLineValidator.IsCommandLineValid(new[] {"osjfjs"}));
        }

        [Test]
        public void TestParseCommandLine9()
        {
            Assert.IsFalse(CommandLineValidator.IsCommandLineValid(new[] {"-osjfjs"}));
        }

        [Test]
        public void TestParseCommandLine10()
        {
            var cli = CommandLineInfo.ParseCommandLine(new[] {"-dw:ssid", "dummy"});

            Assert.IsTrue(cli.Action == CommandLineAction.DisconnectWifi && cli.WifiSSID == "dummy");
        }

        [Test]
        public void TestParseCommandLine11()
        {
            var cli = CommandLineInfo.ParseCommandLine(new[] {"-cw:ssid", "hello"});

            Assert.IsNull(cli); // needs a '-cw:pwd' and password, too
        }

        [Test]
        public void TestParseCommandLine12()
        {
            var cli = CommandLineInfo.ParseCommandLine(new[] {"-cw:ssid", "hello", "-cw:pwd", "world"});

            Assert.IsNotNull(cli);

            Assert.IsTrue(cli.Action == CommandLineAction.ConnectWifi && cli.WifiSSID == "hello" && cli.WifiPassword == "world");
        }

        [Test]
        public void TestParseCommandLine13()
        {
            var cli = CommandLineInfo.ParseCommandLine(new[] {"-cw:ssid", "hello", "-cw:pwd", ""});

            Assert.IsNotNull(cli);

            Assert.IsTrue(cli.Action == CommandLineAction.ConnectWifi && cli.WifiSSID == "hello" &&
                          string.IsNullOrWhiteSpace(cli.WifiPassword));
        }
    }
}