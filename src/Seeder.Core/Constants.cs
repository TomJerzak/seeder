namespace Seeder.Core
{
    public static class Constants
    {
        public const string StorageName = "Seeds";
        public const string SqlExtension = ".sql";

        public static class Command
        {
            public const string Database = "database";
            public const string DatabaseUpdate = "update";
            public const string Scripts = "scripts";
            public const string ScriptsAdd = "add";
            public const string Version = "--version";
            public const string Unknown = "Unknown options/command.";
        }
    }
}
