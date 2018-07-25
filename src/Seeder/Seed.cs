using System;
using System.Collections.Generic;
using System.Reflection;

namespace Seeder
{
    public class Seed
    {
        public static string GenerateScriptName(string scriptName)
        {
            var dateTime = $"{DateTime.Now.Year:0000}{DateTime.Now.Month:00}{DateTime.Now.Date:00}{DateTime.Now.Hour:00}{DateTime.Now.Minute:00}{DateTime.Now.Second:00}_";

            return $"{dateTime}_{scriptName}";
        }

        public static List<string> SortScriptsByName(List<string> scripts)
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

        public static string GetProductVersion()
        {
            return $"{Assembly.GetExecutingAssembly().GetName().Name}_{Assembly.GetExecutingAssembly().GetName().Version}";
        }

        public static Version GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }
    }
}
