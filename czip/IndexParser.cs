using System;
using System.IO;
using System.Text;

namespace czip
{
    public class ParseException : Exception
    {
        public ParseException() { }
        public ParseException(string message) : base(message) { }
        public ParseException(string message, Exception inner) : base(message, inner) { }
    }

    public static class IndexParser
    {
        public const char FS = (char)28;
        public const char GS = (char)29;
        public const char RS = (char)30;
        public const char US = (char)31;

        public static ZipDirectory Parse(BinaryReader stream)
        {
            ZipDirectory root = ParseDirectory(stream);
            return root;
        }

        private static long ByteArrayToLong(byte[] c)
        {
            long res = 0L;
            for (int i = 0; i < 8; i++)
                res = (c[i] << (8 * i)) + res;
            return res;
        }

        private static long ReadLong(BinaryReader stream)
        {
            byte[] c = new byte[8];
            stream.Read(c, 0, c.Length);
            return ByteArrayToLong(c);
        }

        private static ZipFile ParseFile(BinaryReader stream)
        {
            char curChar;
            StringBuilder curField = new StringBuilder();
            ZipFile zfile = new ZipFile();

            // Parse Name
            curChar = (char)stream.Read();
            while (curChar != US)
            {
                curField.Append(curChar);
                curChar = (char)stream.Read();
            }
            zfile.Name = curField.ToString();

            // Parse Start and Size
            zfile.Start = stream.ReadInt64();
            zfile.Size = stream.ReadInt64();

            ConsoleUtil.PrintInfo($"Parsed file \"{zfile.Name}\" from index ({zfile.Start}+{zfile.Size})");
            return zfile;
        }

        private static ZipDirectory ParseDirectory(BinaryReader stream)
        {
            char curChar;
            StringBuilder curField = new StringBuilder();
            ZipDirectory zdir = new ZipDirectory();

            // Parse Name
            while (true)
            {
                curChar = (char)stream.Read();
                if (curChar == GS)
                {
                    zdir.Name = curField.ToString();
                    curField.Clear();
                    break;
                }
                else if (stream.BaseStream.Position >= stream.BaseStream.Length)
                    throw new ParseException("End of stream while parsing");
                curField.Append(curChar);
            }

            // Parse Files
            while (stream.Read() == RS)
            {
                ZipFile nzfile = ParseFile(stream);
                zdir.Files.Add(nzfile);
            }

            // Parse Directories
            while (stream.Read() == RS)
            {
                ZipDirectory nzdir = ParseDirectory(stream);
                zdir.Directories.Add(nzdir);
            }

            ConsoleUtil.PrintInfo(
                $"Parsed directory \"{zdir.Name ?? "UNKNOWN NAME"}\" with " +
                $"{zdir.Directories.Count} subdirectories and {zdir.Files.Count} files");
            return zdir;
        }
    }
}
