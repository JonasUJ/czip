using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace czip
{
    public class ZipFile : IZippable
    {
        public string Name { get; set; }
        public string Extension { get => Path.GetExtension(Name); }
        public long Start;
        public long Size;
        public FileInfo File;

        public ZipFile() { }

        public ZipFile(FileInfo fi)
        {
            File = fi;
            Size = File.Length;
            Name = fi.Name;
        }

        public void CopyFromMappedFile(MemoryMappedFile file)
        {
            if (Size > 0)
            {
                MemoryMappedViewStream viewstream;
                try
                {
                    viewstream = file.CreateViewStream(Start, Size, MemoryMappedFileAccess.Read);
                }
                catch (UnauthorizedAccessException)
                {
                    throw new CorruptionException(
                        "File data is not present in the MemoryMappedFile");
                }
                using (viewstream)
                using (FileStream stream = File.Open(
                        FileMode.Create, FileAccess.Write, FileShare.Read))
                    viewstream.CopyTo(stream);
            }
            else
                using (FileStream _ = File.Create()) { }

        }

        public void CopyToFile(FileStream stream)
        {
            using (FileStream origin = File.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                origin.CopyTo(stream);
            }
        }

        public SerializedData Serialize() {
            SerializedData sd = new SerializedData();
            sd.Add(Name);
            sd.AddUS();
            sd.Add(Start);
            sd.Add(Size);
            return sd;
        }
    }
}
