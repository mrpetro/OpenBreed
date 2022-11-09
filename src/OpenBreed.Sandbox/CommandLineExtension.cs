using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Database.Xml;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

namespace OpenBreed.Sandbox
{
    public static class CommandLineExtension
    {
        #region Public Methods

        public static void SetupCommandLine(this IHostBuilder hostBuilder, string[] args)
        {
            hostBuilder.ConfigureServices((sc) =>
            {
                var dbFilePathOption = new Option<string>
                    (name: "--dbFilePath",
                    description: "Path to the game database file",
                    getDefaultValue: () => "db.xml");

                var legacyFolderPathOption = new Option<string>
                    ("--legacyFolderPath", "Path to legacy game resources folder.");

                var startingLevelOption = new Option<string>
                    ("--startingLevelName", "Name of the starting level.");

                var rootCommand = new RootCommand
                {
                    dbFilePathOption,
                    legacyFolderPathOption,
                    startingLevelOption
                };

                ConfigureXmlDbSettings(rootCommand, args, (result) =>
                {
                    sc.Configure<XmlDbSettings>(xmlDbSettings =>
                    {
                        xmlDbSettings.DbFilePath = result.GetValueForOption(dbFilePathOption);
                    });

                    sc.Configure<EnvironmentSettings>(environmentSettings =>
                    {
                        environmentSettings.LegacyFolderPath = result.GetValueForOption(legacyFolderPathOption);
                    });

                    sc.Configure<GameSettings>(sameSettings =>
                    {
                        sameSettings.StartingLevelName = result.GetValueForOption(startingLevelOption);
                    });
                });
            });
        }

        #endregion Public Methods

        #region Private Methods

        private static void ConfigureXmlDbSettings(
            RootCommand rootCommand,
            string[] args,
            Action<ParseResult> resultProvider)
        {
            rootCommand.SetHandler((handle) =>
            {
                resultProvider.Invoke(handle.ParseResult);
            });

            rootCommand.Invoke(args);
        }

        #endregion Private Methods
    }
}