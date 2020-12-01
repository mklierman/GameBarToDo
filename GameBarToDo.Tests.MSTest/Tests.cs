
using GameBarToDo.Helpers;
using GameBarToDo.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameBarToDo.Tests.MSTest
{
    // TODO WTS: Add appropriate tests
    [TestClass]
    public class Tests
    {

        [TestMethod]
        public void TestInitializingDatabase()
        {
            SQLiteHelper db = new SQLiteHelper();
            Assert.IsTrue(db.InitializeDatabase());
        }

        [TestMethod]
        public void TestAddingListToDatabase()
        {
            SQLiteHelper db = new SQLiteHelper();
            db.InitializeDatabase();
            Assert.AreEqual(db.AddNewListToTable("TestList"), "List created");
        }

        [TestMethod]
        public void TestAddingItemToList()
        {
            SQLiteHelper db = new SQLiteHelper();
            db.InitializeDatabase();
            //Assert.AreEqual(db.AddNewItemToListItemTableByListName("TestItem", "TestList"), "Item Added");
        }

        [TestMethod]
        public void TestAddingNoteToItem()
        {
            SQLiteHelper db = new SQLiteHelper();
            db.InitializeDatabase();
            Assert.AreEqual(db.AddNewNoteToItemTable("TestNote", 0), "Note added");
        }

        [TestMethod]
        public void TestRemovingListFromDatabase()
        {
            SQLiteHelper db = new SQLiteHelper();
            db.InitializeDatabase();
            //Assert.IsTrue(db.RemoveListFromTable("TestList"));
        }

        [TestMethod]
        public void TestAddingList()
        {
            MainViewModel mainViewModel = new MainViewModel();
            //mainViewModel.NewListCommand("Test");
        }

    }
}
