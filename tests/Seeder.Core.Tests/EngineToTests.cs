using System.Collections.Generic;

namespace Seeder.Core.Tests
{
    public class EngineToTests : Engine
    {
        public EngineToTests(ISeeder seeder) : base(seeder) { }

        public new static string GenerateScriptName(string scriptName)
        {
            return Engine.GenerateScriptName(scriptName);
        }

        public new static List<string> SortScriptsByName(List<string> scripts)
        {
            return Engine.SortScriptsByName(scripts);
        }

        public new static List<string> ListOfChanges(List<string> dbSourceScripts, List<string> codeSourceScripts)
        {
            return Engine.ListOfChanges(dbSourceScripts, codeSourceScripts);
        }
    }
}
