using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Seeder.Core.Tests
{
    public class EngineTests : Engine
    {
        public EngineTests(ISeeder seeder = null) : base(seeder)
        {
        }

        [Fact]
        public void generate_script_name()
        {
            var scriptName = GenerateScriptName("Script");

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

            var result = SortScriptsByName(scripts);

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

            var result = ListOfChanges(dbSourceScripts, codeSourceScripts);

            result.Should().HaveCount(2);
            result[0].Should().Be("20180412101920_Script2");
            result[1].Should().Be("20180419182118_Script3");
        }

        [Fact]
        public void get_product_version()
        {
            Constants.ProductVersion.Should().Contain("Seeder_");
        }

        [Fact]
        public void get_available_providers()
        {
            Constants.Providers.Should().HaveCount(2);
            Constants.Providers[0].Should().Be("PostgreSQL");
            Constants.Providers[1].Should().Be("SQLite");
        }
    }
}
