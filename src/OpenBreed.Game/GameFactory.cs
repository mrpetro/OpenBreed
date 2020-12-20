using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game
{
    public class GameFactory : CoreFactory
    {
        public GameFactory()
        {

            manCollection.AddSingleton<IVariableMan>(() => new VariableMan(manCollection.GetManager<ILogger>()));

        }

        public ICore CreateGame(string gameDbFilePath)
        {
            manCollection.AddSingleton<IDatabase>(() => new XmlDatabaseMan(manCollection.GetManager<IVariableMan>(), gameDbFilePath));
            return new Game(manCollection, new GameModulesFactory());
        }
    }
}
