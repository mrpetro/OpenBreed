using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Services
{
    internal class GameServices : IGameServices
    {
        #region Public Constructors

        public GameServices(ITriggerMan triggers,
            ILogger logger,
            IClipMan<IEntity> clips,
            IWorldMan worlds,
            IScriptMan scripts,
            IDataLoaderFactory dataLoaderFactory)
        {
            Triggers = triggers;
            Logger = logger;
            Clips = clips;
            Worlds = worlds;
            Scripts = scripts;
            DataLoaderFactory = dataLoaderFactory;
        }

        #endregion Public Constructors

        #region Public Properties

        public ITriggerMan Triggers { get; }

        public ILogger Logger { get; }

        public IClipMan<IEntity> Clips { get; }

        public IWorldMan Worlds { get; }

        public IScriptMan Scripts { get; }

        public IDataLoaderFactory DataLoaderFactory { get; }

        #endregion Public Properties
    }
}