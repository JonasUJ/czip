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
            IZippable found = Files.Find(z => z.Name == name);
            if (found != null) return found;
            found = Directories.Find(z => z.Name == name);
            return found;
        }

        public string Serialize()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Name}{IndexParser.GS}");

            foreach (ZipDirectory zdir in Directories)
            {
                sb.Append(IndexParser.RS);
                sb.Append(zdir.Serialize());
            }
            sb.Append(IndexParser.GS);

            foreach (ZipFile zfile in Files)
            {
                sb.Append(IndexParser.RS);
                sb.Append(zfile.Serialize());
            }
            sb.Append(IndexParser.GS);

            return sb.ToString();
        }
    }
}
