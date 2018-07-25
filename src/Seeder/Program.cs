using System;
using System.IO;
using System.Linq;

namespace Seeder
{
    class Program
    {
        private static bool _commandWasExecuted = false;

        static void Main(string[] args)
        {
            if (!args.Any())
            {
                WriteConsoleInformation();
                return;
            }

            Run(args);
            
            if (!_commandWasExecuted)
                WriteUnknownCommandInformation();
        }

        private static void Run(string[] args)
        {
            for (var i = 0; i < args.Length; i++)
            {
                if (IsVersionCommand(args[i]))
                {
                    CommandWasExecuted();
                    Console.WriteLine(Seed.GetVersion());
                }

                // TODO: help for scripts
                // if(IsScriptsHelpCommand(...))...
                if (args[i].Equals("scripts") && (i + 1) == args.Length)
                {
                    CommandWasExecuted();
                    Console.WriteLine("Help: TODO...");
                }
                else if (IsScriptsCommand(args[i++]))
                {
                    try
                    {
                        if (args[i++].Equals("add"))
                            CreateScript(args[i]);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        WriteUnknownCommandInformation();
                        CommandWasExecuted();
                    }
                }

                // TODO: dotnet seeder database update
            }
        }

        private static void WriteConsoleInformation()
        {
            Console.WriteLine("\n---------------------------------------------------");
            Console.WriteLine($"| Database Seeder .NET Command-line Tools {Seed.GetVersion()} |");
            Console.WriteLine("---------------------------------------------------\n");
            Console.WriteLine("Usage: dotnet seeder [options] [command]\n");
            Console.WriteLine("Options:");
            Console.WriteLine("--version     Show version information.\n");
            Console.WriteLine("Commands:");
            Console.WriteLine("scripts  Commands to manage scripts.\n");
            Console.WriteLine("Use \"dotnet seeder [command] --help\" for more information about a command.\n");
        }

        private static void WriteUnknownCommandInformation()
        {
            Console.WriteLine("Unknown options/command.");
        }

        private static void CommandWasExecuted()
        {
            _commandWasExecuted = true;
        }

        private static bool IsVersionCommand(string argument)
        {
            return argument.Equals("--version");
        }

        private static bool IsScriptsCommand(string argument)
        {
            return argument.Equals("scripts");
        }

        private static void CreateScript(string scriptName)
        {
            CommandWasExecuted();

            scriptName = Seed.GenerateScriptName(scriptName);

            if (!Directory.Exists("Seeds"))
                Directory.CreateDirectory("Seeds");

            using (var streamWriter = new StreamWriter(File.Create($"Seeds/{scriptName}.sql")))
                streamWriter.WriteLine($"-- {Seed.GetProductVersion()} / {DateTime.Now}");

            Console.WriteLine($"{scriptName} created.");
        }
    }
}
