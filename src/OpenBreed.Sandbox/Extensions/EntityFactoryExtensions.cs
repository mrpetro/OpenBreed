using Microsoft.EntityFrameworkCore.Internal;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Extensions
{
    public static class EntityFactoryExtensions
    {
        #region Private Fields

        private const string PICKABLE_PREFIX = @"ABTA\Templates\Common\Pickables";
        private const string PREFIX = @"ABTA\Templates\Common\Environment";
        private const string PREFIX_L1 = @"ABTA\Templates\L1";
        private const string PREFIX_COMMON = @"ABTA\Templates\Common";

        #endregion Private Fields

        #region Public Methods

        public static IEntity CreateElectricGateVertical(this IEntityFactory entityFactory, int x, int y, string level)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\Common\ElectricGateVertical")
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            return entity;
        }

        public static IEntity CreateElectricGateHorizontal(this IEntityFactory entityFactory, int x, int y, string level)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\Common\ElectricGateHorizontal")
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            return entity;
        }

        public static IEntity CreateDoor(this IEntityFactory entityFactory, int x, int y, string level, string key)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\Common\Door")
                .SetParameter("level", level)
                .SetParameter("key", key)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            return entity;
        }

        public static IEntity CreateMapEntry(this IEntityFactory entityFactory, int x, int y, int entryId, string level, int gfxValue)
        {
            var entryEntity = entityFactory.Create(@"ABTA\Templates\Common\MapEntry")
                .SetParameter("level", level)
                .SetParameter("imageIndex", gfxValue)
                .SetParameter("entryId", entryId)
                .SetParameter("startX", 16 * x)
            .SetParameter("startY", 16 * y)
                .Build();

            return entryEntity;
        }

        public static IEntity CreateMapExit(this IEntityFactory entityFactory, int ix, int iy, int exitId, string level, int gfxValue)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\Common\MapExit")
                .SetParameter("level", level)
                .SetParameter("imageIndex", gfxValue)
                .SetParameter("exitId", exitId)
                .SetParameter("startX", 16 * ix)
                .SetParameter("startY", 16 * iy)
                .Build();

            return entity;
        }

        public static IEntity CreateUnknownCell(this IEntityFactory entityFactory, int x, int y, int actionValue, string level, int gfxValue)
        {
            var path = $@"{PREFIX}\Unknown";

            var entity = entityFactory.Create(path)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            entity.Add(new UnknownCodeComponent(actionValue));

            return entity;
        }

        #endregion Public Methods

        #region Internal Methods

        internal static IEntity CreateVoidCell(this IEntityFactory entityFactory, int x, int y, string level, int gfxValue)
        {
            var path = $@"{PREFIX}\Void";

            var entity = entityFactory.Create(path)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            return entity;
        }

        internal static IEntity CreateSlopeObstacleCell(this IEntityFactory entityFactory, int x, int y, string level, int gfxValue, string slopeDir)
        {
            var path = $@"{PREFIX}\SlopeObstacle";

            var entity = entityFactory.Create(path)
                .SetParameter("slopeDir", slopeDir)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            return entity;
        }

        internal static IEntity CreateFullObstacleCell(this IEntityFactory entityFactory, int x, int y, string level, int gfxValue)
        {
            var path = $@"{PREFIX}\FullObstacle";

            var entity = entityFactory.Create(path)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            return entity;
        }

        internal static IEntity CreateLandMineCell(this IEntityFactory entityFactory, int x, int y, string level, int gfxValue)
        {
            var path = $@"{PREFIX_COMMON}\LandMine";

            var entity = entityFactory.Create(path)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            return entity;
        }

        internal static IEntity CreateActorOnlyObstacleCell(this IEntityFactory entityFactory, int x, int y, string level, int gfxValue)
        {
            var path = $@"{PREFIX}\ActorOnlyObstacle";

            var entity = entityFactory.Create(path)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            return entity;
        }

        public static IEntity CreateTVFlickering(this IEntityFactory entityFactory, int x, int y, string level, int gfxValue)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\L4\TVFlickering")
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            return entity;
        }

        public static IEntity CreateMonsterEating(this IEntityFactory entityFactory, int x, int y, string level, int gfxValue)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\L4\MonsterEating")
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            return entity;
        }

        public static IEntity CreateShipSmoke(this IEntityFactory entityFactory, int x, int y, string level, int gfxValue)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\L1\ShipSmoke")
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            return entity;
        }

        public static IEntity CreateMission(this IEntityFactory entityFactory, string name)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\Common\Mission")
                .SetTag(name)
                .Build();

            return entity;
        }

        public static IEntity CreateHeavyTurret(this IEntityFactory entityFactory, float x, float y)
        {
            var entity = entityFactory.Create($@"ABTA\Templates\L1\Turret")
                .SetParameter("startX", x)
                .SetParameter("startY", y)
                .Build();

            entity.Add(new TrackingComponent(-1));

            return entity;
        }

        public static IEntity CreateItem(this IEntityFactory entityFactory, int x, int y, string name, string level, int gfxValue, string option, string flavor = null)
        {
            var path = $@"{PICKABLE_PREFIX}\{name}";

            var entity = entityFactory.Create(path)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .SetParameter("flavor", flavor)
                .SetParameter("option", option)
                .Build();

            return entity;
        }


        #endregion Internal Methods
    }
}