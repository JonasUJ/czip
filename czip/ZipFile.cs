using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace czip
{
    public class ZipFile : IZippable, IEquatable<ZipFile>
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

        public ZipFile(string name, long start, long size)
        {
            Name = name;
            Start = start;
            Size = size;
            File = null;
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

        public SerializedData Serialize()
        {
            SerializedData sd = new SerializedData();
            sd.Add(Name);
            sd.AddUS();
            sd.Add(Start);
            sd.Add(Size);
            return sd;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ZipFile);
        }

        public bool Equals(ZipFile other)
        {
            return other != null &&
                   Name == other.Name &&
                   Start == other.Start &&
                   Size == other.Size;
        }

        public override int GetHashCode()
        {
            var hashCode = 1898371460;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Start.GetHashCode();
            hashCode = hashCode * -1521134295 + Size.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ZipFile left, ZipFile right)
        {
            return EqualityComparer<ZipFile>.Default.Equals(left, right);
        }

        public static bool operator !=(ZipFile left, ZipFile right)
        {
            return !(left == right);
        }
    }
}
