using Microsoft.VisualStudio.TestTools.UnitTesting;
using czip;

namespace czip.tests
{
    [TestClass()]
    public class ConsoleUtilTests
    {
        private string msg;
        private void AssertEvent(object sender, ConsoleEventArgs e)
        {
            if (e.Message != msg)
            {
                ConsoleUtil.MessageEvent -= AssertEvent;
                ConsoleUtil.InfoEvent -= AssertEvent;
                ConsoleUtil.WarningEvent -= AssertEvent;
                ConsoleUtil.ErrorEvent -= AssertEvent;
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void PrintMessageTest()
        {
            msg = "UnitTests";
            ConsoleUtil.MessageEvent += AssertEvent;
            ConsoleUtil.PrintMessage(msg);
        }

        [TestMethod()]
        public void PrintInfoTest()
        {
            msg = "UnitTests";
            ConsoleUtil.MessageEvent += AssertEvent;
            ConsoleUtil.PrintInfo(msg);
        }

        [TestMethod()]
        public void PrintWarningTest()
        {
            msg = "UnitTests";
            ConsoleUtil.WarningEvent += AssertEvent;
            ConsoleUtil.PrintWarning(msg);
        }

        [TestMethod()]
        public void PrintErrorTest()
        {
            msg = "UnitTests";
            ConsoleUtil.ErrorEvent += AssertEvent;
            ConsoleUtil.PrintError(msg);
        }
    }
}