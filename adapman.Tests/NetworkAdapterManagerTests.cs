using System;
using NUnit.Framework;

namespace adapman.Tests
{
    [TestFixture]
    public class NetworkAdapterManagerTests
    {
        [Test]
        public void TestGetAdapters()
        {
            var adapters = NetworkAdapterManager.GetAdapters();
            foreach (var adapter in adapters)
                Console.WriteLine($"{adapter.ProductName}\t{adapter.Description}");
        }
    }
}