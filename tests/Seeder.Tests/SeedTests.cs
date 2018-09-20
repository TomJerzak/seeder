using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Seeder.Tests
{
    public class SeedTests
    {
        [Fact]
        public void generate_script_name()
        {
            var scriptName = Seed.GenerateScriptName("Script");

            scriptName.Should().Contain($"{DateTime.Now.Year:0000}{DateTime.Now.Month:00}{DateTime.Now.Day:00}{DateTime.Now.Hour:00}{DateTime.Now.Minute:00}{DateTime.Now.Second:00}_");
            scriptName.Should().Contain("_Script");
            scriptName.Length.Should().Be(22);
        }

        [Fact]
        public void sort_scripts_by_name()
        {
            var scripts = new List<string>()
            {
                "20180412101920_Script2",
                "20180328133852_Script1",
                "20180419182118_Script3"
            };

            var result = Seed.SortScriptsByName(scripts);

            result[0].Should().Be("20180328133852_Script1");
            result[1].Should().Be("20180412101920_Script2");
            result[2].Should().Be("20180419182118_Script3");
        }

        [Fact]
        public void list_of_changes()
        {
            var dbSourceScripts = new List<string>()
            {
                "20180328133852_Script1"
            };

            var codeSourceScripts = new List<string>()
            {
                "20180419182118_Script3",
                "20180328133852_Script1",
                "20180412101920_Script2"
            };

            var result = Seed.ListOfChanges(dbSourceScripts, codeSourceScripts);

            result.Should().HaveCount(2);
            result[0].Should().Be("20180412101920_Script2");
            result[1].Should().Be("20180419182118_Script3");
        }

        [Fact]
        public void get_product_version()
        {
            Seed.GetProductVersion().Should().Contain("Seeder_");
        }
    }
}
