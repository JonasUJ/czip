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

        public static Action<ZipFile, object>[] PropertyMap =
        {
            (o, v) => o.Name = v.ToString(),
            (o, v) => o.Start = Convert.ToInt64(v),
            (o, v) => o.Size = Convert.ToInt64(v),
        };

        public ZipFile() { }

        public ZipFile(FileInfo fi)
        {
            File = fi;
            Size = File.Length;
            Name = fi.Name;
        }

        public void PopulateProperty(int index, object value)
        {
            PropertyMap[index](this, value);
        }

        public void CopyFromMappedFile(MemoryMappedFile file)
        {
            if (Size > 0)
                using (MemoryMappedViewStream viewstream = file.CreateViewStream(Start, Size))
                    using (FileStream stream = File.Open(FileMode.Create, FileAccess.Write))
                        viewstream.CopyTo(stream);
            else
                using (FileStream _ = File.Create()) {}

        }

        public void CopyToFile(FileStream stream)
        {
            using (FileStream origin = File.Open(FileMode.Open, FileAccess.Read))
            {
                origin.CopyTo(stream);
            }
        }

        public string Serialize()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Name}{IndexParser.US}" +
                      $"{Start.ToString("D16")}{IndexParser.US}" +
                      $"{Size.ToString("D16")}{IndexParser.US}{IndexParser.FS}");
            return sb.ToString();
        }
    }
}
