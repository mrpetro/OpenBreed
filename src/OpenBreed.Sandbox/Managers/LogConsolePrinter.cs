using Microsoft.Extensions.Hosting;
using OpenBreed.Common.Interface.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Managers
{
    public class LogConsolePrinter : IHostedService
    {
        #region Private Fields

        private readonly ILoggerClient loggerClient;

        #endregion Private Fields

        #region Public Constructors

        public LogConsolePrinter(ILoggerClient loggerClient)
        {
            this.loggerClient = loggerClient;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            StartPrinting();

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        #endregion Public Methods

        #region Private Methods

        private void WriteLineWithColor(string message, ConsoleColor fore)
        {
            Console.ForegroundColor = fore;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void LoggerClient_MessageAdded(Microsoft.Extensions.Logging.LogLevel type, string msg)
        {
            switch (type)
            {
                case Microsoft.Extensions.Logging.LogLevel.Trace:
                    WriteLineWithColor("Trace: " + msg, ConsoleColor.Gray);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    WriteLineWithColor("Debug: " + msg, ConsoleColor.DarkGray);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Information:
                    WriteLineWithColor("Info: " + msg, ConsoleColor.White);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    WriteLineWithColor("Warning: " + msg, ConsoleColor.DarkYellow);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Error:
                    WriteLineWithColor("Error: " + msg, ConsoleColor.Red);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Critical:
                    Console.BackgroundColor = ConsoleColor.Red;
                    WriteLineWithColor("Critical: " + msg, ConsoleColor.White);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.None:
                    break;

                default:
                    break;
            }
        }

        private void StartPrinting()
        {
            loggerClient.MessageAdded += LoggerClient_MessageAdded;
        }

        #endregion Private Methods
    }
}