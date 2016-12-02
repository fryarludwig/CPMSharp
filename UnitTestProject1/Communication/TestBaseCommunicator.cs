using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Communication;
using Common.Messages;
using Common.Users;
using Common.Utilities;

namespace TestCommon.Communication
{
    public class TestCommunicator : BaseCommunicator
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
        }
    }
}
