using System;
using System.IO;

namespace Seeder.Core
{
    public class Engine
    {
        private bool _commandWasExecuted;

        private readonly ISeeder _seeder;

        public Engine(ISeeder seeder)
        {
            _seeder = seeder;
        }

        public void UnknownCommandInformation()
        {
            Console.WriteLine(Constants.Command.Unknown);
        }

        public bool IsCommandExecuted()
        {
            return _commandWasExecuted;
        }

        public void Run(string[] args)
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
                if (args[i].Equals(Constants.Command.Scripts) && (i + 1) == args.Length)
                {
                    CommandWasExecuted();
                    Console.WriteLine("Help: TODO...");
                }
                else if (IsScriptsCommand(args[i]))
                {
                    i++;
                    try
                    {
                        if (args[i++].Equals(Constants.Command.ScriptsAdd))
                            CreateScript(args[i]);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        UnknownCommandInformation();
                        CommandWasExecuted();
                    }
                }

                // TODO: help for database
                // if(IsDatabasesHelpCommand(...))...
                if (args[i].Equals(Constants.Command.Database) && (i + 1) == args.Length)
                {
                    CommandWasExecuted();
                    Console.WriteLine("Help: TODO...");
                }
                else if (args[i++].Equals(Constants.Command.Database))
                {
                    if (args[i].Equals(Constants.Command.DatabaseUpdate))
                    {
                        ISeedRepository seedRepository = new SeedRepository(args[++i]);
                        CommandWasExecuted();

                        _seeder.ExecuteChanges(seedRepository);
                    }
                }
            }
        }

        private void CommandWasExecuted()
        {
            _commandWasExecuted = true;
        }

        private static bool IsVersionCommand(string argument)
        {
            return argument.Equals(Constants.Command.Version);
        }

        private static bool IsScriptsCommand(string argument)
        {
            return argument.Equals(Constants.Command.Scripts);
        }

        private void CreateScript(string scriptName)
        {
            CommandWasExecuted();

            scriptName = Seed.GenerateScriptName(scriptName);

            if (!Directory.Exists(Constants.StorageName))
                Directory.CreateDirectory(Constants.StorageName);

            using (var streamWriter = new StreamWriter(File.Create($"Seeds/{scriptName}{Constants.SqlExtension}")))
                streamWriter.WriteLine($"-- {Seed.GetProductVersion()} / {DateTime.Now}");

            Console.WriteLine($"{scriptName} created.");
        }
    }
}
