using System;
using System.Collections.Generic;
using System.Linq;

namespace czip
{
    public class ZipDirectory : IZippable, IEquatable<ZipDirectory>
    {
        public string Name { get; set; }
        public List<ZipDirectory> Directories = new List<ZipDirectory>();
        public List<ZipFile> Files = new List<ZipFile>();

        public ZipDirectory() { }

        public ZipDirectory(string name, List<ZipDirectory> directories, List<ZipFile> files)
        {
            Name = name;
            Directories = directories;
            Files = files;
        }

        public void OffsetFiles(long offset)
        {
            foreach (ZipFile zfile in AllFiles())
            {
                zfile.Start = offset;
                offset += zfile.Size;
            }
        }

        public IEnumerable<ZipFile> AllFiles()
        {
            foreach (ZipFile zfile in Files)
                yield return zfile;
            foreach (ZipDirectory zdir in Directories)
                foreach (ZipFile zfile in zdir.AllFiles())
                    yield return zfile;
        }

        public IZippable FindByName(string name)
        {
            name = name.ToLower();
            return (IZippable)Files.Find(z => z.Name.ToLower() == name) ??
                   Directories.Find(z => z.Name.ToLower() == name);
        }

        public SerializedData Serialize()
        {
            SerializedData sd = new SerializedData();
            sd.Add(Name);
            sd.AddGS();

            foreach (ZipFile zfile in Files)
            {
                sd.AddRS();
                sd.Add(zfile.Serialize());
            }
            sd.AddGS();

            foreach (ZipDirectory zdir in Directories)
            {
                sd.AddRS();
                sd.Add(zdir.Serialize());
            }
            sd.AddGS();

            return sd;
        }

        public bool DeepEquals(ZipDirectory zdir)
        {
            if (Directories.Count != zdir.Directories.Count) return false;
            if (!Files.SequenceEqual(zdir.Files)) return false;
            for (int i = 0; i < Directories.Count; i++)
                if (!Directories[i].DeepEquals(zdir.Directories[i]))
                    return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ZipDirectory);
        }

        public bool Equals(ZipDirectory other)
        {
            return other != null &&
                   Name == other.Name &&
                   EqualityComparer<List<ZipDirectory>>.Default.Equals(Directories, other.Directories) &&
                   EqualityComparer<List<ZipFile>>.Default.Equals(Files, other.Files);
        }

        public override int GetHashCode()
        {
            var hashCode = 1768900658;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<ZipDirectory>>.Default.GetHashCode(Directories);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<ZipFile>>.Default.GetHashCode(Files);
            return hashCode;
        }

        public static bool operator ==(ZipDirectory left, ZipDirectory right)
        {
            return EqualityComparer<ZipDirectory>.Default.Equals(left, right);
        }

        public static bool operator !=(ZipDirectory left, ZipDirectory right)
        {
            return !(left == right);
        }
    }
}
