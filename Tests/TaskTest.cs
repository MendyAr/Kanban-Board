using NUnit.Framework;
using IntroSE.Kanban.Backend.BusinessLayer;
using System;

namespace IntroSE.Kanban.Tests
{
    public class TaskTest
    {
        private Task task;

        [SetUp]
        public void Setup()
        {
            task = new Task(0, DateTime.Today, "title1", "", DateTime.Today.AddDays(1), "", "", "");
            task.dTask.Persist = false;
        }

        [TestCase("z")]
        [TestCase("!")]
        [TestCase("asdlkjntlkjrewnytlksdfgjklnsdf")]
        [TestCase("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz")]
        public void Title_InBoundsTitle_Success(string title)
        {
            //arrange
            try
            {
                //act
                task.Title = title;
                Assert.AreEqual(title, task.Title, "Requested title was not set");
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}
