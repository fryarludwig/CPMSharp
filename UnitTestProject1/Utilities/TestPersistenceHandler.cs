using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Utilities;
using Common.WorkItems;
using Common.Users;

namespace TestCommon.Utilities
{
    [TestClass]
    public class TestPersistenceHandler
    {
        public string persistentFileName = "./test_persistence.json";

        public bool ValidateEmptySettings(PersistentSettings settings)
        {
            try
            {
                Assert.IsNotNull(settings);
                Assert.IsNotNull(settings.ContractList);
                Assert.IsNotNull(settings.PhaseList);
                Assert.IsNotNull(settings.TaskList);
                Assert.IsNotNull(settings.UserList);
                Assert.IsNotNull(settings.CustomItems);

                Assert.AreEqual(settings.ContractList.Count, 0);
                Assert.AreEqual(settings.PhaseList.Count, 0);
                Assert.AreEqual(settings.TaskList.Count, 0);
                Assert.AreEqual(settings.UserList.Count, 0);
                Assert.AreEqual(settings.CustomItems.Count, 0);

                return true;
            }
            catch
            {
                return false;
            }
        }

        [TestMethod]
        public void PersistentSettingConstructor()
        {
            PersistentSettings settings = new PersistentSettings();
            Assert.IsTrue(ValidateEmptySettings(settings));
        }

        [TestMethod]
        public void PersistentFileHandler_EmptySettings()
        {
            PersistentSettings settings = new PersistentSettings();
            Assert.IsTrue(ValidateEmptySettings(settings));

            PersistentFileHandler fileHandler = new PersistentFileHandler("TestFileHandler");
            Assert.IsTrue(fileHandler.Save(persistentFileName, settings));

            PersistentSettings loadedSettings = fileHandler.Load(persistentFileName);
            Assert.IsTrue(ValidateEmptySettings(loadedSettings));
        }

        [TestMethod]
        public void PersistentFileHandler_PopulatedSettings()
        {
            PersistentSettings settings = new PersistentSettings();
            Assert.IsTrue(ValidateEmptySettings(settings));

            settings.TaskList.Add(new Task());

            settings.PhaseList.Add(new Phase());
            settings.PhaseList.Add(new Phase());

            settings.ContractList.Add(new Contract());
            settings.ContractList.Add(new Contract());
            settings.ContractList.Add(new Contract());

            settings.UserList.Add(new User());
            settings.UserList.Add(new User());
            settings.UserList.Add(new User());
            settings.UserList.Add(new User());

            settings.CustomItems["String_1"] = "SomeValue";
            settings.CustomItems["String_2"] = "AnotherValue";
            settings.CustomItems["Number_1"] = "1";

            PersistentFileHandler fileHandler = new PersistentFileHandler("TestFileHandler");
            Assert.IsTrue(fileHandler.Save(persistentFileName, settings));

            PersistentSettings loadedSettings = fileHandler.Load(persistentFileName);

            Assert.IsNotNull(loadedSettings.ContractList);
            Assert.IsNotNull(loadedSettings.PhaseList);
            Assert.IsNotNull(loadedSettings.TaskList);
            Assert.IsNotNull(loadedSettings.UserList);
            Assert.IsNotNull(loadedSettings.CustomItems);

            Assert.AreEqual(loadedSettings.ContractList.Count, 3);
            Assert.AreEqual(loadedSettings.PhaseList.Count, 2);
            Assert.AreEqual(loadedSettings.TaskList.Count, 1);
            Assert.AreEqual(loadedSettings.UserList.Count, 4);
            Assert.AreEqual(loadedSettings.CustomItems.Count, 3);
        }
    }
}
