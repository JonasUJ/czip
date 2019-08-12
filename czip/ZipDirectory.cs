using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace czip
{
    public class ZipDirectory : IZippable
    {
        public string Name { get; set; }
        public List<ZipDirectory> Directories = new List<ZipDirectory>();
        public List<ZipFile> Files = new List<ZipFile>();

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
            IZippable found = Files.Find(z => z.Name.ToLower() == name);
            if (found != null) return found;
            found = Directories.Find(z => z.Name.ToLower() == name);
            return found;
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
    }
}
