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
                ZipDirectory rootDir = Index(path);
                if (rootDir == null) continue;
                res.AppendLine($"\nIndex of {path}:");
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
                if (fi.Exists)
                {
                    ZipFile zfile = PackFile(fi);
                    if (zfile != null) rootDir.Files.Add(zfile);
                    continue;
                }
                DirectoryInfo di = new DirectoryInfo(path);
                if (di.Exists)
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
                        ConsoleUtil.PrintError("Unable to unzip because the .czip file is corrupt");
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
                        ConsoleUtil.PrintInfo($"Searching for selector: {selector}");
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
                            ConsoleUtil.PrintError(
                                "Unable to unzip because the .czip file is corrupt");
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
            string path = $"{dirPath}\\{dir.Name}";
            ConsoleUtil.PrintInfo($"Unpacking directory {dir.Name}");
            DirectoryInfo di;
            try
            {
                di = new DirectoryInfo(path);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is PathTooLongException)
            {
                throw new CorruptionException($"Directory name \"{dir.Name}\" is invalid");
            }

            if (File.Exists(path))
            if (ConsoleUtil.PromptYN("A file with the same name as the " +
                 $"directory \"{di.FullName}\" being unpacked already exists, remove it?"))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
                {
                    ConsoleUtil.PrintError("Unable delete file.");
                    return;
                }
            }
            else return;
            di.Create();

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
            string path = $"{dirPath}\\{zfile.Name}";
            if (Directory.Exists(path))
            {
                ConsoleUtil.PrintWarning($"Unable to unpack file {zfile.Name} to {path} because " +
                    "a directory with the same name exists.");
                return;
            }
            if (File.Exists(path) && !ConsoleUtil.PromptYN(
                $"File \"{dirPath}\\{zfile.Name}\" already exists, overwrite it?")) return;
            ConsoleUtil.PrintInfo($"Unpacking file {zfile.Name}");
            try
            {
                zfile.File = new FileInfo(path);
            }
            catch (Exception ex) when (ex is ArgumentException ||
                                       ex is PathTooLongException ||
                                       ex is NotSupportedException)
            {
                throw new CorruptionException($"File name \"{zfile.Name}\" is invalid");
            }
            zfile.CopyFromMappedFile(mmf);
        }
    }
}
