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
            using (StreamReader stream = new StreamReader(
                File.OpenRead(path), Encoding.Unicode))
            {
                try
                {
                    ZipDirectory zdir = IndexParser.Parse(stream);
                    return zdir;
                }
                catch (ParseException)
                {
                    ConsoleUtil.PrintError($"Unable to parse index of {path}");
                    return null;
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
                if (rootDir == null) continue;
                res = PPIndexRecursionHelper(rootDir, res);
            }
            return res.ToString();
        }

        private static StringBuilder PPIndexRecursionHelper(
            ZipDirectory curdir,
            StringBuilder builder,
            string curpath = "",
            bool addDirName = false)
        {
            if (addDirName) curpath = $"{curpath}{curdir.Name}/";
            else curpath = $"{curpath}/";
            builder.AppendLine($"DIR  {curpath}");
            foreach (ZipFile zfile in curdir.Files)
                builder.AppendLine($"FILE {curpath}{zfile.Name}");
            foreach (ZipDirectory zdir in curdir.Directories)
                builder = PPIndexRecursionHelper(zdir, builder, curpath, true);
            return builder;
        }

        public static void Zip(IEnumerable<string> paths, string location)
        {
            ZipDirectory rootDir = new ZipDirectory
            {
                Name = Path.GetFileName(paths.First().TrimEnd('\\', '/'))
            };

            foreach (string path in paths)
            {
                ConsoleUtil.PrintMessage($"Zipping {path}");
                FileInfo fi = new FileInfo(path);
                DirectoryInfo di = new DirectoryInfo(path);
                if (fi.Exists)
                {
                    ZipFile zfile = PackFile(fi);
                    if (zfile != null) rootDir.Files.Add(zfile);
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
                    zfile.CopyToFile(fs);
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
                    if (zfile != null) zdir.Files.Add(zfile);
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
            try
            {
                using (FileStream _ =
                    fileinfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read)) { }
            }
            catch (IOException)
            {
                ConsoleUtil.PrintWarning(
                    $"Skipping {fileinfo.FullName} because it cannot be read");
                return null;
            }
            catch (UnauthorizedAccessException)
            {
                ConsoleUtil.PrintWarning(
                    $"Skipping {fileinfo.FullName} because access was denied");
                return null;
            }
            ZipFile zfile = new ZipFile(fileinfo);
            return zfile;
        }

        public static void Unzip(IEnumerable<string> paths)
        {
            foreach (string path in paths)
            {
                ConsoleUtil.PrintMessage($"Unzipping {path}");
                ConsoleUtil.PrintMessage("Parsing index...");
                ZipDirectory rootDir = Index(path);
                if (rootDir == null) continue;

                using (MemoryMappedFile mmf =
                       MemoryMappedFile.CreateFromFile(path, FileMode.Open))
                {
                    ConsoleUtil.PrintMessage("Unpacking...");
                    try
                    {
                        UnpackDirectory(rootDir, mmf, Directory.GetCurrentDirectory());
                    }
                    catch (CorruptionException)
                    {
                        ConsoleUtil.PrintError("The .czip file is corrupt because it does not " +
                            "contain all the data its index points to");
                    }
                }
            }
        }

        public static void Unzip(IEnumerable<string> paths, IEnumerable<string> selectors)
        {
            foreach (string path in paths)
            {
                ZipDirectory rootDir = Index(path);
                if (rootDir == null) continue;

                using (MemoryMappedFile mmf =
                       MemoryMappedFile.CreateFromFile(path, FileMode.Open))
                {
                    foreach (string selector in selectors)
                    {
                        IZippable zip = SelectorSearch(
                            rootDir,
                            selector.Split(
                                new char[] { '\\', '/' },
                                StringSplitOptions.RemoveEmptyEntries));
                        try
                        {
                            if (zip is ZipDirectory zdir)
                            {
                                ConsoleUtil.PrintMessage(
                                    $"Unpacking directory {zdir.Name} from {path}");
                                UnpackDirectory(zdir, mmf, Directory.GetCurrentDirectory());
                            }
                            else if (zip is ZipFile zfile)
                            {
                                ConsoleUtil.PrintMessage(
                                    $"Unpacking file {zfile.Name} from {path}");
                                UnpackFile(zfile, mmf, Directory.GetCurrentDirectory());
                            }
                            else
                            {
                                ConsoleUtil.PrintWarning(
                                    $"Selector {selector} not found in {path}");
                            }
                        }
                        catch (CorruptionException)
                        {
                            ConsoleUtil.PrintError("The .czip file is corrupt because it does " +
                                "not contain all the data its index points to");
                        }
                    }
                }
            }
        }

        private static IZippable SelectorSearch(ZipDirectory root, IEnumerable<string> selector)
        {
            if (selector.Count() == 0) return root;
            ConsoleUtil.PrintInfo($"Searching for {selector.First()} in {root.Name}");
            IZippable zip = root.FindByName(selector.First());
            if (selector.Count() == 1) return zip;
            if (zip is ZipDirectory zdir) return SelectorSearch(zdir, selector.Skip(1));
            return null;
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
