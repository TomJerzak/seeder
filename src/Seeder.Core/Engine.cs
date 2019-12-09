using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

        public void Run(string[] args)
        {
            for (var i = 0; i < args.Length; i++)
            {
                if (IsVersionCommand(args[i]))
                {
                    CommandWasExecuted();
                    Console.WriteLine($"{Constants.Seeder} {Constants.Version.SeederVersion} ({Constants.SeederCore} {Constants.Version.CoreVersion})");
                }

                if (IsProviderArgument(args[i]))
                {
                    CommandWasExecuted();
                    Console.WriteLine($"Available providers: {string.Join(", ", Constants.Providers)}");
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
                        var provider = args[++i];
                        if (provider.Equals(Constants.Provider.PostgreSql, StringComparison.InvariantCultureIgnoreCase))
                        {
                            ISeedRepository seedRepository = new SeedRepository(args[++i]);
                            CommandWasExecuted();

                            _seeder.ExecuteChanges(seedRepository);
                        }
                        else if (provider.Equals(Constants.Provider.SqLite, StringComparison.InvariantCultureIgnoreCase))
                        {
                            ISeedRepository seedRepository = new SqLiteRepository(args[++i]);
                            CommandWasExecuted();

                            _seeder.ExecuteChanges(seedRepository);
                        }
                    }
                }
            }
        }

        public void UnknownCommandInformation()
        {
            Console.WriteLine(Constants.Command.Unknown);
        }

        public bool IsCommandExecuted()
        {
            return _commandWasExecuted;
        }

        private void CommandWasExecuted()
        {
            _commandWasExecuted = true;
        }

        private static bool IsVersionCommand(string argument)
        {
            return argument.Equals(Constants.Command.VersionArgument);
        }

        private static bool IsProviderArgument(string argument)
        {
            return argument.Equals(Constants.Command.ProviderArgument);
        }

        private static bool IsScriptsCommand(string argument)
        {
            return argument.Equals(Constants.Command.Scripts);
        }

        protected void CreateScript(string scriptName)
        {
            CommandWasExecuted();

            scriptName = GenerateScriptName(scriptName);

            if (!Directory.Exists(Constants.StorageName))
                Directory.CreateDirectory(Constants.StorageName);

            using (var streamWriter = new StreamWriter(File.Create($"Seeds/{scriptName}{Constants.SqlExtension}"), Encoding.UTF8))
                streamWriter.WriteLine($"-- {Constants.ProductVersion} / {DateTime.Now}");

            Console.WriteLine($"{scriptName} created.");
        }

        protected static string GenerateScriptName(string scriptName)
        {
            var dateTime = $"{DateTime.Now.Year:0000}{DateTime.Now.Month:00}{DateTime.Now.Day:00}{DateTime.Now.Hour:00}{DateTime.Now.Minute:00}{DateTime.Now.Second:00}_";

            return $"{dateTime}_{scriptName}";
        }

        protected static List<string> SortScriptsByName(List<string> scripts)
        {
            scripts.Sort();

            return scripts;
        }

        public static List<string> ListOfChanges(List<string> dbSourceScripts, List<string> codeSourceScripts)
        {
            codeSourceScripts = SortScriptsByName(codeSourceScripts);

            foreach (var dbSourceScript in dbSourceScripts)
            {
                for (var i = 0; i < codeSourceScripts.Count; i++)
                {
                    if (dbSourceScript.Equals(codeSourceScripts[i]))
                    {
                        codeSourceScripts.RemoveAt(i);
                        break;
                    }
                }
            }

            return codeSourceScripts;
        }
    }
}
