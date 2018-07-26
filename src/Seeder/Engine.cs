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

                // TODO: dotnet seeder database update
                if (args[i].Equals("database"))
                {
                    _seedRepository = new SeedRepository(args[++i]);
                    CommandWasExecuted();

                    Console.WriteLine("\nSeeds:");
                    var seedsHistory = new List<string>();
                    if (_seedRepository.IsExistsSeedsHistory())
                    {
                        seedsHistory = _seedRepository.GetSeedsHistory();
                        foreach (var seed in seedsHistory)
                            Console.WriteLine(seed);
                    }

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

                    ExecuteChanges(seedsHistory, seedsFilesHistory);
                }
            }
        }

        private void ExecuteChanges(List<string> seedsHistory, List<string> seedsFilesHistory)
        {
            Console.WriteLine("\nList of changes:");
            var listOfChanges = Seed.ListOfChanges(seedsHistory, seedsFilesHistory);
            foreach (var seedId in listOfChanges)
            {
                Console.WriteLine(seedId);

                _seedRepository.AddToSeedsHistory(seedId.Replace(".sql", string.Empty), Seed.GetProductVersion());

                var fileStream = new FileStream($"{StorageName}/{seedId}.sql", FileMode.Open);
                using (var streamReader = new StreamReader(fileStream))
                    _seedRepository.RunScript(streamReader.ReadToEnd());
            }
        }

        private void CommandWasExecuted()
        {
            _commandWasExecuted = true;
        }

        private bool IsVersionCommand(string argument)
        {
            return argument.Equals("--version");
        }

        private bool IsScriptsCommand(string argument)
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
