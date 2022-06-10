﻿using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities
{
    public class GenericCellHelper
    {
        #region Private Fields

        private const string PREFIX = @"Defaults\Templates\ABTA\Common\Environment";
        private const string PREFIX_L1 = @"Defaults\Templates\ABTA\L1";

        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;

        #endregion Private Fields

        #region Public Constructors

        public GenericCellHelper(IDataLoaderFactory dataLoaderFactory, IEntityFactory entityFactory)
        {
            this.dataLoaderFactory = dataLoaderFactory;
            this.entityFactory = entityFactory;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddUnknownCell(World world, int x, int y, int actionValue, string level, int gfxValue)
        {
            var path = $@"{PREFIX}\Unknown.xml";

            var pickable = entityFactory.Create(path)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            pickable.Add(new UnknownCodeComponent(actionValue));

            pickable.EnterWorld(world.Id);
        }

        internal void AddVoidCell(World world, int x, int y, string level, int gfxValue)
        {
            var path = $@"{PREFIX}\Void.xml";

            var pickable = entityFactory.Create(path)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            pickable.EnterWorld(world.Id);
        }

        internal void AddSlopeObstacleCell(World world, int x, int y, string level, int gfxValue, string slopeDir)
        {
            var path = $@"{PREFIX}\SlopeObstacle.xml";

            var pickable = entityFactory.Create(path)
                .SetParameter("slopeDir", slopeDir)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            pickable.EnterWorld(world.Id);
        }

        internal void AddFullObstacleCell(World world, int x, int y, string level, int gfxValue)
        {
            var path = $@"{PREFIX}\FullObstacle.xml";

            var pickable = entityFactory.Create(path)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            pickable.EnterWorld(world.Id);
        }

        internal void AddLandMineCell(World world, int x, int y, string level, int gfxValue)
        {
            var path = $@"{PREFIX_L1}\LandMine.xml";

            var pickable = entityFactory.Create(path)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            pickable.EnterWorld(world.Id);
        }

        internal void AddActorOnlyObstacleCell(World world, int x, int y, string level, int gfxValue)
        {
            var path = $@"{PREFIX}\ActorOnlyObstacle.xml";

            var pickable = entityFactory.Create(path)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            pickable.EnterWorld(world.Id);
        }

        #endregion Public Methods
    }
}
