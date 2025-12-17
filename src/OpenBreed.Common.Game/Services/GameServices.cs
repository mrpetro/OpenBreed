using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Interface;
using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Common.Data;
using OpenBreed.Common.Game.Managers;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
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
        private readonly Lazy<IEntityFactory> lazyFactory;

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
            IStampMan stamps,
            ItemsMan items,
            IShapeMan shapes,
            Lazy<IEntityFactory> lazyFactory,
            IEntityTriggerMan entityTriggers)
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
            Items = items;
            Shapes = shapes;
            this.lazyFactory = lazyFactory;
            EntityTriggers = entityTriggers;
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

        public ItemsMan Items { get; }

        public IShapeMan Shapes { get; }

        public IEntityFactory Factory => lazyFactory.Value;

        public IEntityTriggerMan EntityTriggers { get; }

        #endregion Public Properties
    }
}