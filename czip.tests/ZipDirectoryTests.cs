using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace czip.tests
{
    [TestClass()]
    public class ZipDirectoryTests
    {
        private ZipDirectory testdir = new ZipDirectory("testdir",
        new List<ZipDirectory> {
            new ZipDirectory("testdir", new List<ZipDirectory>(), new List<ZipFile> {
                new ZipFile("2", 2, 2),
            })
        },
        new List<ZipFile> {
            new ZipFile("0", 0, 0),
            new ZipFile("1", 1, 1),
        });

        [TestMethod()]
        public void OffsetFilesTest()
        {
            testdir.OffsetFiles(8);
            if (testdir.Files[0].Start != 8 ||
                testdir.Files[1].Start != 8 ||
                testdir.Directories[0].Files[0].Start != 9)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void AllFilesTest()
        {
            if (testdir.AllFiles().Count() != 3)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void FindByNameTest()
        {
            if ((ZipFile)testdir.FindByName("0") != testdir.Files[0] ||
                (ZipDirectory)testdir.FindByName("testdir") != testdir.Directories[0])
            {
                Assert.Fail();
            }
        }

    }
}