using OpenBreed.Common.Tools;
using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Events;
using OpenBreed.Rendering.Interface;
using OpenBreed.Sandbox.Components;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Animation.Interface;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs.Entities;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Common;
using System.Globalization;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Audio.Interface.Data;
using OpenBreed.Common.Interface;
using OpenTK.Mathematics;

namespace OpenBreed.Sandbox.Entities.Door
{
    public class DoorHelper
    {
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;
        private readonly IWorldMan worldMan;

        public DoorHelper(
            IDataLoaderFactory dataLoaderFactory,
            IEntityFactory entityFactory,
            IWorldMan worldMan)
        {
            this.dataLoaderFactory = dataLoaderFactory;
            this.entityFactory = entityFactory;
            this.worldMan = worldMan;
        }

        public IEntity AddVertical(IWorld world, int x, int y, string level, string key)
        {
            var entity = entityFactory.Create(@"Vanilla\ABTA\Templates\Common\DoorVertical.xml")
                .SetParameter("level", level)
                .SetParameter("key", key)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        public static IEntity GetEntityByDataGrid(IEntity entity, IWorldMan worldMan, Vector2i indexOffset)
        {
            var thisdata = entity.Get<MetadataComponent>();
            var pos = entity.Get<PositionComponent>();
            var world = worldMan.GetById(entity.WorldId);
            var dataGrid = world.GetModule<IDataGrid<IEntity>>();
            var indexPos = new Vector2i((int)pos.Value.X / 16, (int)pos.Value.Y / 16);
            var thisEntity = dataGrid.Get(indexPos);
            var indexIndexPos = Vector2i.Add(indexPos, indexOffset);
            var resultEntity = dataGrid.Get(indexIndexPos);

            return resultEntity;
        }

        public IEntity AddDoor(IWorld world, int x, int y, string level, string key)
        {
            var entity = entityFactory.Create(@"Vanilla\ABTA\Templates\Common\Door.xml")
                .SetParameter("level", level)
                .SetParameter("key", key)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        public IEntity AddHorizontal(IWorld world, int x, int y, string level, string key)
        {
            var entity = entityFactory.Create(@"Vanilla\ABTA\Templates\Common\DoorHorizontal.xml")
                .SetParameter("level", level)
                .SetParameter("key", key)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }
    }
}
