using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static ZipDirectory Parse(StreamReader stream)
        {
            ZipDirectory root = ParseDirectory(stream);
            return root;
        }

        private static long CharArrayToLong(char[] c)
        {
            long res = 0L;
            for (int i = 0; i < 8; i++)
                res = (c[i] << 8 * i) + res;
            res -= 32;
            return res;
        }

        private static long ReadLong(StreamReader stream)
        {
            char[] c = new char[8];
            stream.ReadBlock(c, 0, c.Length);
            return CharArrayToLong(c);
        }

        private static ZipFile ParseFile(StreamReader stream)
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
            zfile.Start = ReadLong(stream);
            zfile.Size = ReadLong(stream);

            ConsoleUtil.PrintInfo($"Parsed file \"{zfile.Name}\" from index");
            return zfile;
        }

        private static ZipDirectory ParseDirectory(StreamReader stream)
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
                else if (stream.EndOfStream)
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
