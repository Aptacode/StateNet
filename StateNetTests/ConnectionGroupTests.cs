using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
{
    public class ConnectionGroupTests
    {
        [Test]
        public void ConnectionGroup_Initialise_Empty()
        {
            var group = new ConnectionGroup();
            Assert.AreEqual(0, group.GetAll().Count, "Connection Group Should have no StateDistribution's");
            Assert.AreEqual(0, group.GetAllDistributions().ToList().Count, "Connection Group Should have no StateDistribution's");
            Assert.AreEqual(0, group.GetAllActions().ToList().Count, "Connection Group Should have no Action's");
        }

        [Test]
        public void CanCreateStateDistribution()
        {
            var group = new ConnectionGroup();
            var distribution = group["Next"];
            Assert.AreEqual(distribution, group["Next"]);
        }
        [Test]
        public void DoesNotOverwriteStateDistribution()
        {
            var group = new ConnectionGroup();
            var distribution1 = group["Next"];
            var distribution2 = group["Next"];

            Assert.AreEqual(distribution1, distribution2);
        }

        [Test]
        public void CanGetAllActions()
        {
            var group = new ConnectionGroup();
            var nextDistribution = group["Next"];
            var backDistribution = group["Back"];
            Assert.IsTrue(group.GetAllActions().SequenceEqual(new List<string> { "Next", "Back"}));
        }

        [Test]
        public void CanGetAllConnections()
        {
            var group = new ConnectionGroup();
            var nextDistribution = group["Next"];
            var backDistribution = group["Back"];
            Assert.AreEqual(2, group.GetAll().Count);
        }
    }
}