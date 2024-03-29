﻿using OpenBreed.Common.Tools;
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

        public DoorHelper(IDataLoaderFactory dataLoaderFactory, IEntityFactory entityFactory)
        {
            this.dataLoaderFactory = dataLoaderFactory;
            this.entityFactory = entityFactory;
        }

        public Entity AddVertical(World world, int x, int y, string level, string key)
        {
            var entity = entityFactory.Create(@"Vanilla\ABTA\Templates\Common\DoorVertical.xml")
                .SetParameter("level", level)
                .SetParameter("key", key)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            entity.EnterWorld(world.Id);

            return entity;
        }

        public static Entity GetEntityByDataGrid(Entity entity, IWorldMan worldMan, Vector2i indexOffset)
        {
            var thisdata = entity.Get<MetadataComponent>();
            var pos = entity.Get<PositionComponent>();
            var world = worldMan.GetById(entity.WorldId);
            var dataGrid = world.GetModule<IDataGrid<Entity>>();
            var indexPos = new Vector2i((int)pos.Value.X / 16, (int)pos.Value.Y / 16);
            var thisEntity = dataGrid.Get(indexPos);
            var indexIndexPos = Vector2i.Add(indexPos, indexOffset);
            var resultEntity = dataGrid.Get(indexIndexPos);

            return resultEntity;
        }

        public static Entity GetDoorSecondPart(Entity entity, IWorldMan worldMan, out string type)
        {
            var thisdata = entity.Get<MetadataComponent>();
            var pos = entity.Get<PositionComponent>();
            var world = worldMan.GetById(entity.WorldId);
            var dataGrid = world.GetModule<IDataGrid<Entity>>();
            var indexPos = new Vector2i((int)pos.Value.X / 16, (int)pos.Value.Y / 16);
            var thisEntity = dataGrid.Get(indexPos);
            var downIndexPos = Vector2i.Add(indexPos, new Vector2i(0, 1));
            var downEntity = dataGrid.Get(downIndexPos);

            var downMeta = downEntity.TryGet<MetadataComponent>();

            if (downMeta is not null && downMeta.Name == thisdata.Name)
            {
                type = "Vertical";
                return downEntity;
            }

            var rightIndexPos = Vector2i.Add(indexPos, new Vector2i(1, 0));
            var rightEntity = dataGrid.Get(rightIndexPos);
            var rightMeta = rightEntity.TryGet<MetadataComponent>();

            if (rightMeta is not null && rightMeta.Name == thisdata.Name)
            {
                type = "Horizontal";
                return rightEntity;
            }

            type = null;
            return null;
        }

        public Entity AddDoor(World world, int x, int y, string level, string key)
        {
            var entity = entityFactory.Create(@"Vanilla\ABTA\Templates\Common\Door.xml")
                .SetParameter("level", level)
                .SetParameter("key", key)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            entity.EnterWorld(world.Id);

            return entity;
        }

        public Entity AddHorizontal(World world, int x, int y, string level, string key)
        {
            var entity = entityFactory.Create(@"Vanilla\ABTA\Templates\Common\DoorHorizontal.xml")
                .SetParameter("level", level)
                .SetParameter("key", key)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            entity.EnterWorld(world.Id);

            return entity;
        }
    }
}
