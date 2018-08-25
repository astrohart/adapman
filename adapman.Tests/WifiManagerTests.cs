using System;
using NUnit.Framework;

namespace adapman.Tests
{
    [TestFixture]
    public class WifiManagerTests
    {
        [Test]
        public void TestIsConnected()
        {
            Console.WriteLine($"IsConnected = {WifiManager.IsConnected}");
        }

        [Test]
        public void TestConnect()
        {
            WifiManager.Connect("The Worst Astronomer", "$!#WIFIn2ikp141");
        }

        [Test]
        public void TestDisconnect()
        {
            WifiManager.Disconnect("The Worst Astronomer");
        }
    }
}