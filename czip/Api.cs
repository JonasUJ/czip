using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace czip
{
    public class CorruptionException : Exception
    {
        public CorruptionException() { }
        public CorruptionException(string message) : base(message) { }
        public CorruptionException(string message, Exception inner) : base(message, inner) { }
    }

    public static class Api
    {
        public static ZipDirectory Index(string path)
        {
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(path, FileMode.Open))
            {
                FileInfo fi = new FileInfo(path);
                using (StreamReader stream = new StreamReader(
                    mmf.CreateViewStream(0, fi.Length), Encoding.Unicode))
                {
                    ZipDirectory zdir = IndexParser.ParseStream(stream);
                    return zdir;
                }
            }
        }

        public static string PPIndex(IEnumerable<string> paths)
        {
            StringBuilder res = new StringBuilder();
            foreach (string path in paths)
            {
                res.AppendLine($"\nIndex of {path}:");
                ZipDirectory rootDir = Index(path);
                res = PPIndexRecursionHelper(rootDir, res);
            }
            return res.ToString();
        }

        private static StringBuilder PPIndexRecursionHelper(
            ZipDirectory curdir, StringBuilder builder, int indents = 0)
        {
            builder.AppendLine($"{new String(' ', indents)}{curdir.Name}/");
            indents += 4;
            foreach (ZipFile zfile in curdir.Files)
                builder.AppendLine($"{new String(' ', indents)}{zfile.Name}");
            foreach (ZipDirectory zdir in curdir.Directories)
                builder = PPIndexRecursionHelper(zdir, builder, indents);
            return builder;
        }

        public static void Zip(IEnumerable<string> paths, string location)
        {
            ZipDirectory rootDir = new ZipDirectory
            {
                Name = Path.GetFileName(paths.First())
            };

            foreach (string path in paths)
            {
                ConsoleUtil.PrintMessage($"Zipping {path}");
                FileInfo fi = new FileInfo(path);
                DirectoryInfo di = new DirectoryInfo(path);
                if (fi.Exists)
                {
                    ZipFile zfile = PackFile(fi);
                    rootDir.Files.Add(zfile);
                }
                else if (di.Exists)
                {
                    ZipDirectory zdir = PackDirectory(di);
                    if (zdir != null) rootDir.Directories.Add(zdir);
                }
            }

            int offset = Encoding.Unicode.GetByteCount(rootDir.Serialize());
            rootDir.OffsetFiles(offset);
            using (FileStream fs = File.Create(location))
            {
                fs.Write(Encoding.Unicode.GetBytes(rootDir.Serialize()), 0, offset);
                foreach (ZipFile zfile in rootDir.AllFiles())
                {
                    try
                    {
                        zfile.CopyToFile(fs);
                    }
                    catch (IOException)
                    {
                        ConsoleUtil.PrintWarning(
                            $"Skipping {zfile.File.FullName} because it cannot be read");
                    }
                    catch (UnauthorizedAccessException)
                    {
                        ConsoleUtil.PrintWarning(
                            $"Skipping {zfile.File.FullName} because access was denied");
                    }
                }
                ConsoleUtil.PrintMessage($"Created .czip file {location}");
            }
        }

        public static ZipDirectory PackDirectory(DirectoryInfo dirinfo)
        {
            ConsoleUtil.PrintInfo($"Packing directory {dirinfo.FullName}");
            ZipDirectory zdir = new ZipDirectory { Name = dirinfo.Name };
            try
            {
                foreach (DirectoryInfo di in dirinfo.GetDirectories())
                {
                    ZipDirectory ndir = PackDirectory(di);
                    if (ndir != null) zdir.Directories.Add(ndir);
                }
                foreach (FileInfo fi in dirinfo.GetFiles())
                {
                    ZipFile zfile = PackFile(fi);
                    zdir.Files.Add(zfile);
                }
                return zdir;
            }
            catch (UnauthorizedAccessException)
            {
                ConsoleUtil.PrintWarning($"Skipping {dirinfo.FullName} because access was denied");
                return null;
            }
        }

        public static ZipFile PackFile(FileInfo fileinfo)
        {
            ConsoleUtil.PrintInfo($"Packing file {fileinfo.FullName}");
            ZipFile zfile = new ZipFile(fileinfo);
            return zfile;
        }

        public static void Unzip(IEnumerable<string> paths)
        {
            foreach (string path in paths)
            {
                ConsoleUtil.PrintMessage($"Unzipping {path}");
                FileInfo fi = new FileInfo(path);
                ZipDirectory rootDir;
                using (StreamReader stream = new StreamReader(
                    fi.OpenRead(), Encoding.Unicode))
                {
                    ConsoleUtil.PrintMessage("Parsing index...");
                    rootDir = IndexParser.ParseStream(stream);
                }
                using (MemoryMappedFile mmf =
                       MemoryMappedFile.CreateFromFile(path, FileMode.Open))
                {
                    ConsoleUtil.PrintMessage("Unpacking...");
                    UnpackDirectory(rootDir, mmf, fi.Directory.FullName);
                }
            }
        }

        public static void UnpackDirectory(
            ZipDirectory dir, MemoryMappedFile mmf, string dirPath)
        {
            ConsoleUtil.PrintInfo($"Unpacking directory {dir.Name}");
            DirectoryInfo di = Directory.CreateDirectory($"{dirPath}\\{dir.Name}");
            foreach (ZipDirectory subdir in dir.Directories)
            {
                UnpackDirectory(subdir, mmf, di.FullName);
            }
            foreach (ZipFile file in dir.Files)
            {
                UnpackFile(file, mmf, di.FullName);
            }
        }

        public static void UnpackFile(ZipFile zfile, MemoryMappedFile mmf, string dirPath)
        {
            if (File.Exists($"{dirPath}\\{zfile.Name}") && !ConsoleUtil.PromptYN(
                $"File \"{dirPath}\\{zfile.Name}\" exists, override it?")) return;
            ConsoleUtil.PrintInfo($"Unpacking file {zfile.Name}");
            zfile.File = new FileInfo($"{dirPath}\\{zfile.Name}");
            zfile.CopyFromMappedFile(mmf);
        }
    }
}
