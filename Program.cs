using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

[assembly: AssemblyUsage(
    "   czip -p <PATH>... [-u [-s <PATH>...]] [-i] [-y]",
    "   czip -p path/to/dir",
    "   czip -p file1.txt file2.txt dir1 dir2",
    "   czip -p file.czip --unzip",
    "   czip -p file.czip --unzip --select path/to/dir path/to/file.txt")]

namespace czip
{
    class Program
    {
        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        public static Stopwatch stopwatch = new Stopwatch();

        private static bool isHelpOrVersion;
        private static bool printTime = false;

        public class Options
        {
            [Option('p', "path", Required = true,
                HelpText = "Path to directories and files or .czip files")]
            public IEnumerable<string> Path { get; set; }

            [Option('u', "unzip",
                HelpText = "Unzip selected .czip files")]
            public bool Unzip { get; set; }

            [Option('s', "select",
                HelpText = "Select individual files and folders from the index to unzip")]
            public IEnumerable<string> Select { get; set; }

            [Option('i', "index",
                HelpText = "Print the index of .czip files to the console")]
            public bool Index { get; set; }

            [Option('v', "verbose", HelpText = "Increase verbosity to also print info")]
            public bool Verbose { get; set; }

            [Option('y', HelpText = "Agree to all prompts")]
            public bool Yes { get; set; }
        }

        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                stopwatch.Start();
                ConsoleUtil.stopwatch = stopwatch;
                Parser.Default.ParseArguments<Options>(args)
                    .WithNotParsed(errs => HandleErrors(errs))
                    .WithParsed(opts => RunOptions(opts));
                stopwatch.Stop();

                if (printTime) Console.WriteLine($"Time elapsed: {stopwatch.Elapsed}");
                Console.Write("Press any key to exit . . . ");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Sorry, but the console will be unavailable until the program " +
                    " has been closed :(");
                FreeConsole();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }

        private static void RunOptions(Options opts)
        {
            if (isHelpOrVersion) return;
            ConsoleUtil.AgreeToPrompts = opts.Yes;
            ConsoleUtil.Verbose = opts.Verbose;

            if (opts.Verbose)
                ConsoleUtil.PrintInfo("Increased verbosity");

            if (!VerifyOptions(opts))
                ConsoleUtil.PrintError("Process ended unsuccessfully");
            else if (opts.Index)
                Console.WriteLine(Api.PPIndex(opts.Path));
            else if (opts.Unzip)
            {
                if (opts.Select.Count() != 0)
                    Api.Unzip(opts.Path, opts.Select, Directory.GetCurrentDirectory());
                else
                    Api.Unzip(opts.Path, Directory.GetCurrentDirectory());
                printTime = true;
            }
            else
            {
                string firstPath = Path.GetFullPath(opts.Path.First());
                string finalName = Directory.GetCurrentDirectory() + "\\" +
                    $"{Path.GetFileName(firstPath.TrimEnd('\\', '/'))}.czip";
                if (File.Exists(finalName))
                {
                    if (ConsoleUtil.PromptYN(
                            $"File {finalName} already exists, delete and create it anew?"))
                        File.Delete(finalName);
                    else
                    {
                        ConsoleUtil.PrintWarning("Process was unable to finish");
                        return;
                    }

                }
                Api.Zip(opts.Path, finalName);
                printTime = true;
            }
        }

        private static bool VerifyOptions(Options opts)
        {
            try
            {
                foreach (string p in opts.Path)
                {
                    Path.GetFullPath(p);
                }
            }
            catch (Exception ex) when (ex is ArgumentException ||
                                       ex is NotSupportedException ||
                                       ex is PathTooLongException)
            {
                ConsoleUtil.PrintError("An argument to --path is invalid: " + ex.Message);
                if (ex is ArgumentException)
                    ConsoleUtil.PrintWarning(
                        "Are you escaping a quote? e.g. \"\\path to\\dir\\\" instead of " +
                        "\"\\path to\\dir\\\\\"");
                return false;
            }

            if (opts.Index && (opts.Unzip || opts.Select.Count() != 0))
            {
                ConsoleUtil.PrintError("The --index option cannot be used with other options.");
                return false;
            }
            if (opts.Select.Count() != 0 && !opts.Unzip)
            {
                ConsoleUtil.PrintError("Cannot use the --select option without the --unzip option");
                return false;
            }
            foreach (string p in opts.Path)
            {
                var filePath = new FileInfo(p);
                var dirPath = new DirectoryInfo(p);
                if (!filePath.Exists && !dirPath.Exists)
                {
                    ConsoleUtil.PrintError(
                        $"Path \"{p}\" points to neither a file nor a directory.");
                    return false;
                }
                if (opts.Unzip && !filePath.Exists)
                {
                    ConsoleUtil.PrintError(
                        "--path must point to a .czip file when using the --unzip option");
                    return false;
                }
                if (opts.Index && !filePath.Exists)
                {
                    ConsoleUtil.PrintError(
                        $"--path must point to a file and not a directory ({p})");
                    return false;
                }
            }
            return true;
        }

        private static void HandleErrors(IEnumerable<Error> errs)
        {
            if (errs.Any(e
                => e.GetType() == typeof(HelpRequestedError)
                || e.GetType() == typeof(VersionRequestedError)))

                isHelpOrVersion = true;
            else
                isHelpOrVersion = false;
        }
    }
}
