using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace czip.tests
{
    [TestClass()]
    public class ApiTests
    {
        private string testfiles = "../../../testfiles/";
        private string PPIndex = @"
Index of {TESTFILES}testdir.czip:
DIR  /
DIR  /testdir/
FILE /testdir/testfile
DIR  /testdir/testdir/
FILE /testdir/testdir/testfile
";

        private ZipDirectory testdir = new ZipDirectory
        (
            "testdir",
            new List<ZipDirectory>
            {
                    new ZipDirectory(
                        "testdir",
                        new List<ZipDirectory>
                        {
                            new ZipDirectory(
                            "testdir",
                            new List<ZipDirectory>(),
                            new List<ZipFile>
                            {
                                new ZipFile("testfile", 95L, 11L)
                            })
                        },
                        new List<ZipFile>
                        {
                            new ZipFile("testfile", 84L, 11L)
                        })
            },
            new List<ZipFile> { }
        );

        [TestMethod()]
        public void IndexTest()
        {
            if (!Api.Index($"{testfiles}testdir.czip").DeepEquals(testdir))
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void IndexNullTest()
        {
            ZipDirectory zdir;
            zdir = Api.Index($"{testfiles}corrupt.czip");
            if (zdir != null) Assert.Fail();
        }

        [TestMethod()]
        public void PPIndexTest()
        {
            string res = Api.PPIndex(new string[] { $"{testfiles}testdir.czip" });
            if (res != PPIndex.Replace("{TESTFILES}", testfiles)) Assert.Fail();
            res = Api.PPIndex(new string[] { $"{testfiles}testdir.czip", $"{testfiles}testdir.czip" });
            if (res != PPIndex.Replace("{TESTFILES}", testfiles) + PPIndex.Replace("{TESTFILES}", testfiles)) Assert.Fail();
        }

        [TestMethod()]
        public void ZipTest()
        {
            try
            {
                Api.Zip(new string[] { $"{testfiles}testdir" }, $"{testfiles}ZipTest.czip");
                if (!Api.Index($"{testfiles}ZipTest.czip").DeepEquals(testdir))
                {
                    Assert.Fail();
                }
            }
            finally
            {
                File.Delete($"{testfiles}ZipTest.czip");
            }
        }

        [TestMethod()]
        public void PackDirectoryTest()
        {
            ZipDirectory zdir = Api.PackDirectory(new DirectoryInfo($"{testfiles}testdir"));
            zdir.OffsetFiles(84);
            if (!zdir.DeepEquals(testdir.Directories[0]))
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void PackFileTest()
        {
            ZipFile zfile = Api.PackFile(new FileInfo($"{testfiles}testdir/testfile"));
            ZipFile tfile = new ZipFile("testfile", 0, 11);
            if (zfile != tfile)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void UnzipTest()
        {
            try
            {
                Api.Unzip(new string[] { $"{testfiles}testdir.czip" }, $"{testfiles}testresults");
                if (!(
                    Directory.Exists($"{testfiles}testresults/testdir") &&
                    Directory.Exists($"{testfiles}testresults/testdir/testdir") &&
                    Directory.Exists($"{testfiles}testresults/testdir/testdir/testdir") &&
                    File.Exists($"{testfiles}testresults/testdir/testdir/testfile") &&
                    File.Exists($"{testfiles}testresults/testdir/testdir/testdir/testfile")))
                {
                    Assert.Fail();
                }
                using (StreamReader stream = new StreamReader(File.Open($"{testfiles}testresults/testdir/testdir/testfile", FileMode.Open)))
                {
                    if (stream.ReadLine() != "testcontent")
                    {
                        Assert.Fail();
                    }
                }
                using (StreamReader stream = new StreamReader(File.Open($"{testfiles}testresults/testdir/testdir/testdir/testfile", FileMode.Open)))
                {
                    if (stream.ReadLine() != "testcontent")
                    {
                        Assert.Fail();
                    }
                }
            }
            finally
            {
                DirectoryInfo di = new DirectoryInfo($"{testfiles}testresults/testdir");
                di.Delete(true);
            }
        }

        [TestMethod()]
        public void UnzipSelectorTest()
        {
            try
            {
                Api.Unzip(new string[] { $"{testfiles}testdir.czip" }, new string[] { "/testdir/testdir", "/testdir/testfile" }, $"{testfiles}testresults");
                if (!(
                    Directory.Exists($"{testfiles}testresults//testdir") &&
                    File.Exists($"{testfiles}testresults/testfile") &&
                    File.Exists($"{testfiles}testresults/testdir/testfile")))
                {
                    Assert.Fail();
                }
                using (StreamReader stream = new StreamReader(File.Open($"{testfiles}testresults/testdir/testfile", FileMode.Open)))
                {
                    if (stream.ReadLine() != "testcontent")
                    {
                        Assert.Fail();
                    }
                }
                using (StreamReader stream = new StreamReader(File.Open($"{testfiles}testresults/testfile", FileMode.Open)))
                {
                    if (stream.ReadLine() != "testcontent")
                    {
                        Assert.Fail();
                    }
                }
            }
            finally
            {
                Directory.Delete($"{testfiles}testresults/testdir", true);
                File.Delete($"{testfiles}testresults/testfile");
            }
        }
    }
}