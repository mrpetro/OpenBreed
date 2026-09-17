using OpenBreed.Common.Game.Services;
using OpenBreed.Core;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Systems.Door;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Rendering.Components;
using OpenBreed.Wecs.Worlds;
using System.Windows.Controls;

namespace OpenBreed.Sandbox.Loaders
{
    public class DoorEntityLoader : IMapWorldEntityLoader
    {
        #region Public Fields

        public const int DOOR_RED = 28;
        public const int DOOR_GREEN = 29;
        public const int DOOR_BLUE = 30;

        #endregion Public Fields

        #region Private Fields

        private readonly IWorldMan worldMan;
        private readonly IEntityFactory entityFactory;
        private readonly IGameServices gameServices;

        #endregion Private Fields

        #region Public Constructors

        public DoorEntityLoader(IWorldMan worldMan, IEntityFactory entityFactory, IGameServices gameServices)
        {
            this.worldMan = worldMan ?? throw new System.ArgumentNullException(nameof(worldMan));
            this.entityFactory = entityFactory ?? throw new System.ArgumentNullException(nameof(entityFactory));
            this.gameServices = gameServices ?? throw new System.ArgumentNullException(nameof(gameServices));
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntity Load(MapMapper mapper, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, IWorld world)
        {
            var key = default(string);

            switch (templateName)
            {
                case "DoorStandard":
                    key = "";
                    break;

                case "DoorRed":
                    key = "KeycardRed";
                    break;

                case "DoorGreen":
                    key = "KeycardGreen";
                    break;

                case "DoorBlue":
                    key = "KeycardBlue";
                    break;
            }

            var rightValue = MapLegacyDataLoader.GetActionCellValue(map.Layout, ix + 1, iy);
            var rightAction = map.GetAction(rightValue);

            if (rightAction?.Name == templateName)
            {
                //Door horizontal
                visited[ix, iy] = true;
                visited[ix + 1, iy] = true;
                var entity = CreateDoor(ix, iy, "Horizontal");
                entity.SetMetadata("RequiredKey", key);
                entity.SetMetadata("Flavor", "Horizontal");
                entity.SetMetadata("Level", mapper.Level);
                entity.SetState(string.IsNullOrEmpty(key) ? DoorState.Closed : DoorState.Locked);
                gameServices.Worlds.RequestAddEntity(entity, world.Id);
                return entity;
            }

            var downValue = MapLegacyDataLoader.GetActionCellValue(map.Layout, ix, iy + 1);
            var downAction = map.GetAction(downValue);

            if (downAction?.Name == templateName)
            {
                //Door vertical
                visited[ix, iy] = true;
                visited[ix, iy + 1] = true;
                var entity = CreateDoor(ix, iy, "Vertical");
                entity.SetMetadata("RequiredKey", key);
                entity.SetMetadata("Flavor", "Vertical");
                entity.SetMetadata("Level", mapper.Level);
                entity.SetState(string.IsNullOrEmpty(key) ? DoorState.Closed : DoorState.Locked);
                gameServices.Worlds.RequestAddEntity(entity, world.Id);

                return entity;
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private IEntity CreateDoorPart(int x, int y, string key)
        {
            var entity = gameServices.Factory.Create($@"ABTA\Templates\Common\DoorPart")
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            return entity;
        }

        private IEntity CreateDoor(int x, int y, string orientation)
        {
            var entity = gameServices.Factory.Create($@"ABTA\Templates\Common\Door{orientation}")
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            return entity;
        }

        #endregion Private Methods
    }
}