using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace czip
{
    public class SerializedData
    {
        public long Length { get => stream.Length; }

        private MemoryStream stream;
        private BinaryWriter writer;

        public SerializedData()
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
        }

        public void AddFS() { Add((char)28); }
        public void AddGS() { Add((char)29); }
        public void AddRS() { Add((char)30); }
        public void AddUS() { Add((char)31); }

        public void Add(char data)
        {
            writer.Write(data);
        }

        public void Add(string data)
        {
            byte[] bytes = Encoding.Default.GetBytes(data);
            writer.Write(bytes);
        }

        public void Add(long data)
        {
            data += 32;
            writer.Write(data);
        }

        public void Add(SerializedData data)
        {
            data.CopyTo(stream);
        }

        public void CopyTo(MemoryStream stream)
        {
            this.stream.Position = 0;
            this.stream.CopyTo(stream);
        }

        public void CopyTo(FileStream stream)
        {
            this.stream.Position = 0;
            this.stream.CopyTo(stream);
        }
    }
}
