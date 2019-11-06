using Microsoft.VisualStudio.TestTools.UnitTesting;
using czip;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace czip.tests
{
    [TestClass()]
    public class IndexParserTests
    {
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
        public void ParseTest()
        {
            using (FileStream stream = File.Open("../../../testfiles/testdir.czip", FileMode.Open))
            {
                BinaryReader bstream = new BinaryReader(stream);
                ZipDirectory zdir = IndexParser.Parse(bstream);
                if (!zdir.DeepEquals(testdir)) Assert.Fail();
            }
        }
    }
}