using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Abstractions;
using OpenBreed.Audio.Abstractions.Managers;
using OpenBreed.Common.Data;
using OpenBreed.Common.Game.Managers;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Scripting.Abstractions;
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
            IEntityClassMan classes,
            Lazy<IScriptMan> lazyScripts,
            IDataLoaderFactory dataLoaderFactory,
            TextsDataProvider texts,
            ISoundMan sounds,
            IStampMan stamps,
            ItemsMan items,
            IShapeMan shapes,
            IRenderingMan renders,
            Lazy<IEntityFactory> lazyFactory,
            IEntityTriggerMan entityTriggers,
            IEventsMan events,
            IPaletteMan palettes)
        {
            Triggers = triggers;
            Logger = logger;
            Clips = clips;
            Worlds = worlds;
            Entities = entities;
            Classes = classes;
            this.lazyScripts = lazyScripts;
            DataLoaderFactory = dataLoaderFactory;
            Texts = texts;
            Sounds = sounds;
            Stamps = stamps;
            Items = items;
            Shapes = shapes;
            Renders = renders;
            this.lazyFactory = lazyFactory;
            EntityTriggers = entityTriggers;
            Events = events;
            Palettes = palettes;
        }

        #endregion Public Constructors

        #region Public Properties

        public ITriggerMan Triggers { get; }

        public ILogger Logger { get; }

        public IClipMan<IEntity> Clips { get; }

        public IWorldMan Worlds { get; }

        public IEntityMan Entities { get; }

        public IEntityClassMan Classes { get; }

        public IScriptMan Scripts => lazyScripts.Value;

        public IRenderingMan Renders { get; }

        public IDataLoaderFactory DataLoaderFactory { get; }

        public TextsDataProvider Texts { get; }

        public ISoundMan Sounds { get; }

        public IStampMan Stamps { get; }

        public ItemsMan Items { get; }

        public IShapeMan Shapes { get; }

        public IEntityFactory Factory => lazyFactory.Value;

        public IEntityTriggerMan EntityTriggers { get; }

        public IEventsMan Events { get; }

        public IPaletteMan Palettes { get; }


        #endregion Public Properties
    }
}