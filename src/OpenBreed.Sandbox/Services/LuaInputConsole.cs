using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Gui.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OpenBreed.Sandbox.Services
{
    internal class LuaInputConsole : IHostedService
    {
        #region Private Fields

        private readonly IScriptMan scriptMan;
        private readonly CollisionVisualizingOptions visualizingOptions;

        #endregion Private Fields

        #region Public Constructors

        public LuaInputConsole(IScriptMan scriptMan, CollisionVisualizingOptions visualizingOptions)
        {
            this.scriptMan = scriptMan ?? throw new ArgumentNullException(nameof(scriptMan));
            this.visualizingOptions = visualizingOptions;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => ReadInputAsync(cancellationToken)).ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        #endregion Public Methods

        #region Private Methods

        private async Task ReadInputAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            while (!cancellationToken.IsCancellationRequested)
            {
                Console.SetCursorPosition(0, Console.BufferHeight - 1);
                Console.Write("Command: ");
                var commandLine = Console.ReadLine().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (!commandLine.Any())
                {
                    continue;
                }

                var command = commandLine.First();

                var exit = false;

                switch (command.ToLower())
                {
                    case "exit":
                        exit = true;
                        break;

                    case "collisions":
                        var options = commandLine.Skip(1).Take(1);

                        if (!options.Any())
                        {
                            Console.WriteLine("Missing option to 'collisions' command.");
                            continue;
                        }

                        var option = options.First().ToLower();

                        switch (option)
                        {
                            case "show":
                                visualizingOptions.Enabled = true;
                                continue;
                            case "hide":
                                visualizingOptions.Enabled = false;
                                continue;
                            default:
                                Console.WriteLine($"Invalid option ('{option}'). Accepted options: show, hide");
                                continue;
                        }
                    default:
                        break;
                }

                if (exit)
                {
                    break;
                }
                ;

                try
                {
                    scriptMan.RunString(command);
                }
                catch (NLua.Exceptions.LuaException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        #endregion Private Methods
    }
}