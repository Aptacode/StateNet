using Aptacode.StateNet.Tests.Helpers;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.Network.Connection
{
    public class ConnectionTests
    {
        [Test]
        public void ConstructorSetsValues()
        {
            var from = "A";
            var input = "Next";
            var to = "B";
            var weight = 1;

            var connection = DummyConnections.Generate(from, input, to, weight);

            Assert.AreEqual(from, connection.Source.Name);
            Assert.AreEqual(input, connection.Input.Name);
            Assert.AreEqual(to, connection.Target.Name);
            Assert.AreEqual(weight, connection.ConnectionWeight.Evaluate(null));

            Assert.AreEqual($"{from}({input})->({to}:{weight})", connection.ToString());
        }
    }
}