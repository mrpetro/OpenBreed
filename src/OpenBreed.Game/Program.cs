using OpenBreed.Core;
using OpenBreed.Database.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game
{
    class Program
    {
        private const string ABHC_AMIGA_GAME_DB_FILE_NAME = "GameDatabase.ABHC.xml";
        private const string ABSE_AMIGA_GAME_DB_FILE_NAME = "GameDatabase.ABSE.xml";
        private const string ABTA_PC_GAME_DB_FILE_NAME = "GameDatabase.ABTA.EPF.xml";

        static void Main(string[] args)
        {
            if (!args.Any())
                return;

            var gameDbFileName = args[0];
            var execFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var gameDbFilePath = Path.Combine(execFolderPath, gameDbFileName);

            var gameFactory = new GameFactory();
            var game = gameFactory.CreateGame(gameDbFilePath);
            game.Run();     
        }

    }
}
