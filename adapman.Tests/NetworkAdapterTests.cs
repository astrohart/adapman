using System.Linq;
using NUnit.Framework;

namespace adapman.Tests
{
    [TestFixture]
    public class NetworkAdapterTests
    {
        [Test]
        public void TestEnable()
        {
            var adapters = NetworkAdapterManager.GetAdapters();
            if (!adapters.Any())
                return;

            foreach (var adapter in adapters)
                NetworkAdapterManager.Enable(adapter);
        }

        [Test]
        public void TestDisable()
        {
            var adapters = NetworkAdapterManager.GetAdapters();
            if (!adapters.Any())
                return;

            foreach(var adapter in adapters)
                NetworkAdapterManager.Disable(adapter);
        }
    }
}