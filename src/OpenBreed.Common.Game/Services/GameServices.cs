using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Interface;
using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Services
{
    internal class GameServices : IGameServices
    {
        #region Private Fields

        private readonly Lazy<IScriptMan> lazyScripts;

        #endregion Private Fields

        #region Public Constructors

        public GameServices(ITriggerMan triggers,
            ILogger logger,
            IClipMan<IEntity> clips,
            IWorldMan worlds,
            IEntityMan entities,
            Lazy<IScriptMan> lazyScripts,
            IDataLoaderFactory dataLoaderFactory,
            TextsDataProvider texts,
            ISoundMan sounds,
            IStampMan stamps)
        {
            Triggers = triggers;
            Logger = logger;
            Clips = clips;
            Worlds = worlds;
            Entities = entities;
            this.lazyScripts = lazyScripts;
            DataLoaderFactory = dataLoaderFactory;
            Texts = texts;
            Sounds = sounds;
            Stamps = stamps;
        }

        #endregion Public Constructors

        #region Public Properties

        public ITriggerMan Triggers { get; }

        public ILogger Logger { get; }

        public IClipMan<IEntity> Clips { get; }

        public IWorldMan Worlds { get; }

        public IEntityMan Entities { get; }

        public IScriptMan Scripts => lazyScripts.Value;

        public IDataLoaderFactory DataLoaderFactory { get; }

        public TextsDataProvider Texts { get; }

        public ISoundMan Sounds { get; }

        public IStampMan Stamps { get; }

        #endregion Public Properties
    }
}