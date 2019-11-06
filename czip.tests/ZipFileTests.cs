using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace czip.tests
{
    [TestClass()]
    public class ZipFileTests
    {
        private string testfiles = "../../../testfiles/";

        [TestMethod()]
        public void CopyFromMappedFileTest()
        {
            try
            {
                ZipFile zfile = new ZipFile();
                zfile.Start = 0;
                zfile.Size = 11;
                zfile.File = new FileInfo($"{testfiles}testresults/testfile");
                using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile($"{testfiles}testdir/testfile", FileMode.Open))
                {
                    zfile.CopyFromMappedFile(mmf);
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
                File.Delete($"{testfiles}testresults/testfile");
            }
        }

        [TestMethod()]
        public void CopyFromMappedFileThrowTest()
        {
            ZipFile zfile = new ZipFile();
            zfile.Start = 0;
            zfile.Size = 12;
            zfile.File = new FileInfo($"{testfiles}testresults/testfile");
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile($"{testfiles}testdir/testfile", FileMode.Open))
            {
                try
                {
                    zfile.CopyFromMappedFile(mmf);
                    Assert.Fail();
                }
                catch (CorruptionException) { }
            }
        }

        [TestMethod()]
        public void CopyToFileTest()
        {
            ZipFile zfile = new ZipFile(new FileInfo($"{testfiles}testdir/testfile"));
            try
            {
                using (FileStream fs = File.Open($"{testfiles}testresults/testfile", FileMode.OpenOrCreate))
                {
                    zfile.CopyToFile(fs);
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
                File.Delete($"{testfiles}testresults/testfile");
            }
        }
    }
}