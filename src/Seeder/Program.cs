using System;
using System.Linq;
using Seeder.Core;

namespace Seeder
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any())
            {
                ConsoleInformation();
                return;
            }

            var engine = new Engine(new Core.Seeder());

            engine.Run(args);

            if (!engine.IsCommandExecuted())
                engine.UnknownCommandInformation();
        }

        private static void ConsoleInformation()
        {
            Console.WriteLine("\n---------------------------------------------------");
            Console.WriteLine($"| Database Seeder .NET Command-line Tools {Engine.GetVersion()} |");
            Console.WriteLine("---------------------------------------------------\n");
            Console.WriteLine("Usage: dotnet seeder [options] [command]\n");
            Console.WriteLine("Options:");
            Console.WriteLine("--version     Show version information.\n");
            Console.WriteLine("Commands:");
            Console.WriteLine("scripts  Commands to manage scripts.\n");
            Console.WriteLine("database Commands to manage the database.\n");
            Console.WriteLine("Use \"dotnet seeder [command] --help\" for more information about a command.\n");
        }
    }
}
