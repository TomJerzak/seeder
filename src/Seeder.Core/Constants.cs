using System.Collections.Generic;

namespace Seeder.Core
{
    public static class Constants
    {
        public const string Seeder = "Seeder";
        public const string SeederCore = "Core";
        public const string ProductVersion = Seeder + "_" + Version.SeederVersion + " (" + SeederCore + "_" + Version.CoreVersion + ")";
        public const string StorageName = "Seeds";
        public const string SqlExtension = ".sql";

        public static class Command
        {
            public const string Database = "database";
            public const string DatabaseUpdate = "update";
            public const string Scripts = "scripts";
            public const string ScriptsAdd = "add";
            public const string VersionArgument = "--version";
            public const string ProviderArgument = "--provider";
            public const string Unknown = "Unknown options/command.";
        }

        public static class Version
        {
            public const string SeederVersion = "1.0.14";

            public const string CoreVersion = "1.0.10";
        }

        public static readonly List<string> Providers = new List<string>()
        {
            Provider.PostgreSql,
            Provider.SqLite
        };

        public static class Provider
        {
            public const string PostgreSql = "PostgreSQL";
            public const string SqLite = "SQLite";
        }

    }
}
