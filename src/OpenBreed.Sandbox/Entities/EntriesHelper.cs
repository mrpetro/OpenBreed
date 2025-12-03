using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Extensions;
using OpenBreed.Common;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Sandbox.Entities.Viewport;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Sandbox.Systems;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Scripting;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Gui.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Scripting.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Common.Game.Wecs.Extensions;


namespace OpenBreed.Sandbox.Entities
{
    public class EntriesHelper
    {
        #region Public Fields

        public const string SPRITE_WORLD_ENTRY = "Atlases/Sprites/World/Entry";

        public const string SPRITE_WORLD_EXIT = "Atlases/Sprites/World/Exit";

        #endregion Public Fields

        #region Private Fields

        private readonly IWorldMan worldMan;

        private readonly IEntityMan entityMan;
        private readonly ITriggerMan triggerMan;
        private readonly IClipMan<IEntity> clipMan;

        private readonly IEntityFactory entityFactory;

        private readonly IEventsMan eventsMan;

        private readonly ICollisionMan<IEntity> collisionMan;

        private readonly IJobsMan jobsMan;

        private readonly ViewportCreator viewportCreator;
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IScriptMan scriptMan;
        private readonly ILogger logger;
        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public EntriesHelper(
            IWorldMan worldMan,
            IEntityMan entityMan,
            ITriggerMan triggerMan,
            IClipMan<IEntity> clipMan,
            IEntityFactory entityFactory,
            IEventsMan eventsMan,
            ICollisionMan<IEntity> collisionMan,
            IJobsMan jobsMan,
            ViewportCreator viewportCreator,
            IDataLoaderFactory dataLoaderFactory,
            IScriptMan scriptMan,
            ILogger logger,
            IGameServices services)
        {
            this.worldMan = worldMan;
            this.entityMan = entityMan;
            this.triggerMan = triggerMan;
            this.clipMan = clipMan;
            this.entityFactory = entityFactory;
            this.eventsMan = eventsMan;
            this.collisionMan = collisionMan;
            this.jobsMan = jobsMan;
            this.viewportCreator = viewportCreator;
            this.dataLoaderFactory = dataLoaderFactory;
            this.scriptMan = scriptMan;
            this.logger = logger;
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntity AddMapEntry(IWorld world, int x, int y, int entryId, string level, int gfxValue)
        {
            var entryEntity = entityFactory.Create(@"ABTA\Templates\Common\MapEntry")
                .SetParameter("level", level)
                .SetParameter("imageIndex", gfxValue)
                .SetParameter("entryId", entryId)
                .SetParameter("startX", 16 * x)
            .SetParameter("startY", 16 * y)
                .Build();

            worldMan.RequestAddEntity(entryEntity, world.Id);

            return entryEntity;
        }

        public IEntity AddMapExit(IWorld world, int ix, int iy, int exitId, string level, int gfxValue)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\Common\MapExit")
                .SetParameter("level", level)
                .SetParameter("imageIndex", gfxValue)
                .SetParameter("exitId", exitId)
                .SetParameter("startX", 16 * ix)
                .SetParameter("startY", 16 * iy)
                .Build();

            worldMan.RequestAddEntity(entity, world.Id);
            return entity;
        }

        public void ExecuteHeroEnter(IEntity heroEntity, string worldName, int entryId)
        {
            var task = Task.Create((t) => services.AddToWorld(t, heroEntity, worldName));

            task.Then((t) => services.PlayerCharacterEnter(t, heroEntity, entryId));

            task.Start();
        }

        #endregion Public Methods
    }
}