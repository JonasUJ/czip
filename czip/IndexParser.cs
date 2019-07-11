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
        public const char FS = (char) 28;
        public const char GS = (char) 29;
        public const char RS = (char) 30;
        public const char US = (char) 31;

        public static ZipDirectory ParseStream(StreamReader stream)
        {
            ZipDirectory zdir = new ZipDirectory();
            zdir.Directories.Add(ParseDirectory(stream));
            return zdir;
        }

        public static ZipFile ParseFile(StreamReader stream)
        {
            int i = 0;
            char curChar;
            StringBuilder curField = new StringBuilder();
            ZipFile zfile = new ZipFile();
            do
            {
                curChar = (char) stream.Read();
                if (curChar == US)
                {
                    try
                    {
                        zfile.PopulateProperty(i, curField.ToString());
                    }
                    catch (IndexOutOfRangeException) {}
                    curChar = (char) 0;
                    curField.Clear();
                    i++;
                }
                else
                {
                    curField.Append(curChar);
                }
            } while (curChar != FS);
            if (i != ZipFile.PropertyMap.Length)
                ConsoleUtil.PrintWarning(
                    $"File \"{zfile.Name ?? "UNKNOWN NAME"}\" has an unexpected amount of " +
                    $"properties ({i}, should be {ZipFile.PropertyMap.Length})");
            ConsoleUtil.PrintInfo(
                $"Parsed file \"{zfile.Name ?? "UNKNOWN NAME"}\" from index");
            return zfile;
        }

        public static ZipDirectory ParseDirectory(StreamReader stream)
        {
            char curChar;
            StringBuilder curField = new StringBuilder();
            ZipDirectory zdir = new ZipDirectory();

            // Parse Name
            while (true) {
                curChar = (char) stream.Read();
                if (curChar == GS)
                {
                    zdir.Name = curField.ToString();
                    curChar = (char) 0;
                    curField.Clear();
                    break;
                }
                curField.Append(curChar);
            }

            // Parse Directories
            while (stream.Read() == RS)
            {
                ZipDirectory nzdir = ParseDirectory(stream);
                zdir.Directories.Add(nzdir);
            }

            // Parse Files
            while (stream.Read() == RS)
            {
                ZipFile nzfile = ParseFile(stream);
                zdir.Files.Add(nzfile);
            }

            ConsoleUtil.PrintInfo(
                $"Parsed directory \"{zdir.Name ?? "UNKNOWN NAME"}\" with " +
                $"{zdir.Directories.Count} subdirectories and {zdir.Files.Count} files");
            return zdir;
        }
    }
}
