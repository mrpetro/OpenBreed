using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game
{
    public class GameFactory
    {
        private readonly ILogger logger;
        private readonly VariableMan variables;
        private readonly XmlDatabaseMan databaseMan;


        public GameFactory()
        {
            this.logger = new DefaultLogger();
            this.variables = new VariableMan(logger);
            this.databaseMan = new XmlDatabaseMan(variables);
        }

        public ICore CreateGame(string gameDbFilePath)
        {
            databaseMan.Open(gameDbFilePath);
            return new Game(databaseMan, variables, logger, new GameModulesFactory());
        }
    }
}
