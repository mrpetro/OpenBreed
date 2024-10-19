using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Rendering.Interface;
using OpenBreed.Common.Game.Wecs.Components;
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

        private const string PREFIX = @"ABTA\Templates\Common\Environment";
        private const string PREFIX_L1 = @"ABTA\Templates\L1";
        private const string PREFIX_COMMON = @"ABTA\Templates\Common";

        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;
        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public GenericCellHelper(
            IDataLoaderFactory dataLoaderFactory,
            IEntityFactory entityFactory,
            IWorldMan worldMan)
        {
            this.dataLoaderFactory = dataLoaderFactory;
            this.entityFactory = entityFactory;
            this.worldMan = worldMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntity AddUnknownCell(IWorld world, int x, int y, int actionValue, string level, int gfxValue)
        {
            var path = $@"{PREFIX}\Unknown";

            var entity = entityFactory.Create(path)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            entity.Add(new UnknownCodeComponent(actionValue));

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        internal IEntity AddVoidCell(IWorld world, int x, int y, string level, int gfxValue)
        {
            var path = $@"{PREFIX}\Void";

            var entity = entityFactory.Create(path)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        internal IEntity AddSlopeObstacleCell(IWorld world, int x, int y, string level, int gfxValue, string slopeDir)
        {
            var path = $@"{PREFIX}\SlopeObstacle";

            var entity = entityFactory.Create(path)
                .SetParameter("slopeDir", slopeDir)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        internal IEntity AddFullObstacleCell(IWorld world, int x, int y, string level, int gfxValue)
        {
            var path = $@"{PREFIX}\FullObstacle";

            var entity = entityFactory.Create(path)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        internal IEntity AddLandMineCell(IWorld world, int x, int y, string level, int gfxValue)
        {
            var path = $@"{PREFIX_COMMON}\LandMine";

            var entity = entityFactory.Create(path)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        internal IEntity AddActorOnlyObstacleCell(IWorld world, int x, int y, string level, int gfxValue)
        {
            var path = $@"{PREFIX}\ActorOnlyObstacle";

            var entity = entityFactory.Create(path)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        #endregion Public Methods
    }
}
