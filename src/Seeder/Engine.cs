using System;
using System.Collections.Generic;
using System.IO;

namespace Seeder
{
    internal class Engine
    {
        private const string StorageName = "Seeds";
        private ISeedRepository _seedRepository;
        private bool _commandWasExecuted;
        
        public void UnknownCommandInformation()
        {
            Console.WriteLine("Unknown options/command.");
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
                if (args[i].Equals("scripts") && (i + 1) == args.Length)
                {
                    CommandWasExecuted();
                    Console.WriteLine("Help: TODO...");
                }
                else if (IsScriptsCommand(args[i]))
                {
                    i++;
                    try
                    {
                        if (args[i++].Equals("add"))
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
                if (args[i].Equals("database") && (i + 1) == args.Length)
                {
                    CommandWasExecuted();
                    Console.WriteLine("Help: TODO...");
                }
                else if (args[i++].Equals("database"))
                {
                    if (args[i].Equals("update"))
                    {
                        _seedRepository = new SeedRepository(args[++i]);
                        CommandWasExecuted();

                        ExecuteChanges(GetSeedsHistory(), GetSeedsFilesHistory());
                    }
                }
            }
        }

        private static List<string> GetSeedsFilesHistory()
        {
            var seedsFilesHistory = new List<string>();

            Console.WriteLine("\nFiles:");
            var files = new DirectoryInfo(StorageName).GetFiles();
            foreach (var file in files)
            {
                if (file.Extension.Equals(".sql"))
                {
                    seedsFilesHistory.Add(file.Name.Replace(".sql", string.Empty));
                    Console.WriteLine(file.Name);
                }
            }

            return seedsFilesHistory;
        }

        private List<string> GetSeedsHistory()
        {
            var seedsHistory = new List<string>();

            Console.WriteLine("\nSeeds:");
            if (_seedRepository.IsExistsSeedsHistory())
            {
                seedsHistory = _seedRepository.GetSeedsHistory();
                foreach (var seed in seedsHistory)
                    Console.WriteLine(seed);
            }

            return seedsHistory;
        }

        private void ExecuteChanges(List<string> seedsHistory, List<string> seedsFilesHistory)
        {
            Console.WriteLine("\nList of changes:");
            var listOfChanges = Seed.ListOfChanges(seedsHistory, seedsFilesHistory);
            foreach (var seedId in listOfChanges)
            {
                Console.WriteLine(seedId);

                var fileStream = new FileStream($"{StorageName}/{seedId}.sql", FileMode.Open);
                using (var streamReader = new StreamReader(fileStream))
                    _seedRepository.RunScript(streamReader.ReadToEnd());

                _seedRepository.AddToSeedsHistory(seedId.Replace(".sql", string.Empty), Seed.GetProductVersion());
            }
        }

        private void CommandWasExecuted()
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

        private void CreateScript(string scriptName)
        {
            CommandWasExecuted();

            scriptName = Seed.GenerateScriptName(scriptName);

            if (!Directory.Exists(StorageName))
                Directory.CreateDirectory(StorageName);

            using (var streamWriter = new StreamWriter(File.Create($"Seeds/{scriptName}.sql")))
                streamWriter.WriteLine($"-- {Seed.GetProductVersion()} / {DateTime.Now}");

            Console.WriteLine($"{scriptName} created.");
        }
    }
}
