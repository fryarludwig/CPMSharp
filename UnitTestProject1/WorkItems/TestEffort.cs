using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.WorkItems;

namespace TestCommon.WorkItems
{
    [TestClass]
    public class TestEffort
    {
        [TestMethod]
        public void EffortFactory()
        {
            Effort testEffort1 = Effort.GetNewEffort(1, 2, 3, 4);
            Assert.IsNotNull(testEffort1);
            Assert.AreEqual(testEffort1.TimeElapsed.TotalSeconds, 0);
            Assert.AreEqual(testEffort1.UserId, 1);
            Assert.AreEqual(testEffort1.ContractId, 2);
            Assert.AreEqual(testEffort1.PhaseId, 3);
            Assert.AreEqual(testEffort1.TaskId, 4);
        }

        [TestMethod]
        public void EffortTimeElapsed()
        {
            Effort testEffort1 = Effort.GetNewEffort(1, 2, 3, 4);
            Assert.IsNotNull(testEffort1);
            Assert.AreEqual(testEffort1.TimeElapsed.TotalSeconds, 0);

            Assert.IsTrue(testEffort1.StartEffort());

            System.Threading.Thread.Sleep(1000); // wait for 1 second

            Assert.IsTrue(testEffort1.StopEffort());
            Assert.IsTrue(testEffort1.TimeElapsed.TotalSeconds - 1 <= 0.002);

            Console.WriteLine(testEffort1.TimeElapsed.TotalSeconds);
        }
    }
}
