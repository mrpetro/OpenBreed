using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Audio.Interface;
using OpenBreed.Database.Xml;
using OpenBreed.Wecs.Services;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;

namespace OpenBreed.Sandbox
{
    public static class CommandLineExtension
    {
        #region Public Methods

        public static void SetupCommandLine(this IHostBuilder hostBuilder, string[] args)
        {
            hostBuilder.ConfigureServices((sc) =>
            {
                var dbFilePathOption = new Option<string>("--dbFilePath")
                {
                    Description = "Path to the game database file",
                    DefaultValueFactory = (a) => "db.xml"
                };

                var legacyFolderPathOption = new Option<string>("--legacyFolderPath")
                {
                    Description = "Path to legacy game resources folder."
                };

                var startingLevelOption = new Option<string>("--startingLevelName")
                {
                    Description = "Name of the starting level."
                };

                var disableAudioOption = new Option<bool>("--disableAudio")
                {
                    Description = "Disable all game audio."
                };

                var rootCommand = new RootCommand
                {
                    dbFilePathOption,
                    legacyFolderPathOption,
                    startingLevelOption,
                    disableAudioOption
                };

            ConfigureXmlDbSettings(rootCommand, args, (result) =>
                {
                    sc.Configure<XmlDbSettings>(xmlDbSettings =>
                    {
                        xmlDbSettings.DbFilePath = result.GetValue(dbFilePathOption);
                    });

                    sc.Configure<XmlEntityTemplateLoaderSettings>(xmlDbSettings =>
                    {
                        xmlDbSettings.DataDirPath = Path.GetDirectoryName(result.GetValue(dbFilePathOption));
                    });

                    sc.Configure<EnvironmentSettings>(settings =>
                    {
                        settings.LegacyFolderPath = result.GetValue(legacyFolderPathOption);
                    });

                    sc.Configure<GameSettings>(settings =>
                    {
                        settings.StartingLevelName = result.GetValue(startingLevelOption);
                    });

                    sc.Configure<AudioSettings>(settings =>
                    {
                        settings.DisableSound = result.GetValue(disableAudioOption);
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
            rootCommand.SetAction((parseResult) =>
            {
                resultProvider.Invoke(parseResult);
            });

            var parseResult = rootCommand.Parse(args);
            parseResult.Invoke();
        }

        #endregion Private Methods
    }
}