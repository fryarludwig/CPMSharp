using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Communication;
using Common.Messages;
using Common.Users;
using Common.Utilities;
using System.Threading;

namespace TestCommon.Communication
{
    public class TestCommunicator : NetworkClient
    {
        public TestCommunicator() : base("Tester")
        {
            HasRun = false;
        }

        protected override void Run()
        {
            HasRun = true;
        }

        public bool HasRun { get; set; }
    }

    [TestClass]
    public class TestBaseCommunicator
    {
        [TestMethod]
        public void BaseComInstance()
        {
            TestCommunicator tester = new TestCommunicator();
            Assert.IsFalse(tester.HasRun);
            Assert.IsNotNull(tester.LocalEndpoint);

            tester.Start();
            Thread.Sleep(250);

            Assert.IsTrue(tester.HasRun);
            Assert.IsTrue(tester.IsActive);

            tester.Stop();
            Thread.Sleep(250);

            Assert.IsFalse(tester.IsActive);
        }
    }
}
