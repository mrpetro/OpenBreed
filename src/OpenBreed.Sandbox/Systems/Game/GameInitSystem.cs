using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Interface;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Model.Actions;
using OpenBreed.Model.Maps;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Scripting.Abstractions;
using OpenBreed.Wecs.Abstractions.Attributes;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Physics.Components;
using OpenBreed.Wecs.Rendering.Components;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml.Linq;

namespace OpenBreed.Sandbox.Systems.Game
{
    internal class GameInitSystem :
        IEventSystem<WorldInitialized>
    {
        private readonly IGameServices services;
        private readonly ITileMan tileMan;
        private readonly IEntityFactory entityFactory;
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IMapDataLoader mapDataLoader;
        private readonly IRepositoryProvider repositoryProvider;
        private readonly IBuilderFactory builderFactory;
        private readonly MapsDataProvider mapsDataProvider;

        public GameInitSystem(
            IGameServices services,
            ITileMan tileMan,
            IEntityFactory entityFactory,
            IDataLoaderFactory dataLoaderFactory,
            IRepositoryProvider repositoryProvider,
            MapsDataProvider mapsDataProvider,
            IMapDataLoader mapDataLoader,
            IBuilderFactory builderFactory)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.tileMan = tileMan ?? throw new ArgumentNullException(nameof(tileMan));
            this.entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
            this.dataLoaderFactory = dataLoaderFactory ?? throw new ArgumentNullException(nameof(dataLoaderFactory));
            this.repositoryProvider = repositoryProvider ?? throw new ArgumentNullException(nameof(repositoryProvider));
            this.mapsDataProvider = mapsDataProvider ?? throw new ArgumentNullException(nameof(mapsDataProvider));
            this.mapDataLoader = mapDataLoader ?? throw new ArgumentNullException(nameof(mapDataLoader));
            this.builderFactory = builderFactory ?? throw new ArgumentNullException(nameof(builderFactory));
        }

        public void OnEvent(
            [TargetWorldAsSourceFilter]
            [SourceWorldWithNameFilter(WorldNames.Game)]
            WorldInitialized e,
            IWorld world)
        {
            var dbMap = repositoryProvider.GetRepository<IDbMap>().GetById(world.Name);

            if (dbMap is null)
            {
                throw new Exception($"Missing Map: {world.Name}");
            }

            var map = mapsDataProvider.GetMap(dbMap.Id);

            if (map is null)
            {
                throw new Exception($"Map model asset '{dbMap.DataRef}' could not be loaded.");
            }

            LoadMapEntities(world, dbMap, map);

            var johnPlayerEntity = services.Entities.GetByTag("John").FirstOrDefault();

            services.ExecuteHeroEnter(johnPlayerEntity, world.Name, 0);
        }

        private void LoadMapEntities(IWorld world, IDbMap dbMap, MapModel map)
        {

            var layout = map.Layout;
            var visited = new bool[layout.Width, layout.Height];

            var cellSize = layout.CellSize;

            var tileGridComponent = builderFactory.GetBuilder<TileGridComponentBuilder>()
                .SetGrid(layout.Width, layout.Height, 1, cellSize)
                .Build();

            var dataGridComponent = builderFactory.GetBuilder<DataGridComponentBuilder>()
                .SetGrid(layout.Width, layout.Height)
            .Build();

            var collisionComponent = builderFactory.GetBuilder<CollisionComponentBuilder>()
                .SetStaticGrid(layout.Width, layout.Height, cellSize)
                .Build();

            var mapEntity = services.Entities.Create()
                .SetTag($"Maps")
                .AddComponent(new StampPutterComponent())
                .AddComponent(tileGridComponent)
                .AddComponent(dataGridComponent)
                .AddComponent(collisionComponent)
                .Build();

            var mapper = new MapMapper(dbMap.TileSetRef);

            var atlasId = tileMan.GetByName(mapper.Level).Id;
            var gfxLayer = layout.GetLayerIndex(MapLayerType.Gfx);
            var actionLayer = layout.GetLayerIndex(MapLayerType.Action);

            services.Worlds.RequestAddEntity(mapEntity, world.Id);

            var paletteEntityTag = $"Palettes/{dbMap.Id}";
            var paletteEntity = services.Entities.GetByTag(paletteEntityTag).FirstOrDefault();

            if (paletteEntity is not null)
                services.Worlds.RequestAddEntity(paletteEntity, world.Id);

            for (int iy = 0; iy < layout.Height; iy++)
            {
                for (int ix = 0; ix < layout.Width; ix++)
                {
                    var cellValues = layout.GetCellValues(ix, iy);
                    var gfxValue = cellValues[gfxLayer];
                    var actionValue = cellValues[actionLayer];

                    var action = map.GetAction(actionValue);

                    var indexPos = new Vector2i(ix, iy);

                    tileGridComponent.Grid.ModifyTile(indexPos, atlasId, gfxValue);

                    if (action is null)
                        continue;

                    var cellEntity = LoadCellEntity(mapper, map, visited, ix, iy, world, action, gfxValue);

                    if (cellEntity is null)
                        continue;

                    dataGridComponent.Grid.Set(indexPos, cellEntity.Id);

                    //Check if cell entity has static body
                    //if (cellEntity.Contains<PositionComponent>() &&
                    //    cellEntity.Contains<BodyComponent>() &&
                    //    !cellEntity.Contains<VelocityComponent>())
                    //{
                    //    mapEntity.AddEntityToStatics(cellEntity);
                    //}

                    //if (mapper.Map(actionValue, gfxValue, out string templaneName, out string flavor))
                    //    LoadCellEntity(mapper, map, visited, ix, iy, world, templaneName, flavor, gfxValue);
                }
            }

            //Process trough all not visited
            for (int iy = 0; iy < layout.Height; iy++)
            {
                for (int ix = 0; ix < layout.Width; ix++)
                {
                    if (visited[ix, iy])
                        continue;

                    var cellValues = layout.GetCellValues(ix, iy);
                    var gfxValue = cellValues[gfxLayer];
                    var actionValue = cellValues[actionLayer];
                    var indexPos = new Vector2i(ix, iy);

                    var cellEntity = LoadUnknownCodeCell(mapper, map, visited, ix, iy, gfxValue, actionValue, world);

                    if (cellEntity is not null)
                        dataGridComponent.Grid.Set(indexPos, cellEntity.Id);
                }
            }

            AddMission(world);
            AddDirector(world, dbMap.ScriptRef);

            //DEBUG entities
            AddCursor(world);

            //triggerMan.OnWorldInitialized(world, () =>
            //{
            //}, singleTime: true);
        }


        private void AddCursor(IWorld world)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\Common\Hud\Cursor")
                .SetParameter("posX", 0.0f)
                .SetParameter("posY", 0.0f)
                .Build();

            services.Worlds.RequestAddEntity(entity, world.Id);
        }

        private void AddMission(IWorld world)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\Common\Mission")
                //.SetParameter("scriptId", "Vanilla/Common/Mission")
                .SetTag("Mission")
                .Build();

            services.Worlds.RequestAddEntity(entity, world.Id);
        }

        private void AddDirector(IWorld world, string scriptId)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\Common\Director")
                .SetParameter("scriptId", scriptId)
                .SetTag("Director")
                .Build();

            services.Worlds.RequestAddEntity(entity, world.Id);
        }

        private IEntity LoadCellEntity(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, IWorld world, ActionModel action, int gfxValue)
        {
            if (visited[ix, iy])
                return null;

            if (mapDataLoader.TryGetEntityLoader(action.Name, out IMapWorldEntityLoader entityLoader))
                return entityLoader.Load(mapAssets, map, visited, ix, iy, action.Name, "", gfxValue, world);

            services.Logger.LogWarning("Missing loader for action '{0}'.", action);
            return null;
        }

        private void LoadCellEntity(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, IWorld world, string templateName, string flavor, int gfxValue)
        {
            if (visited[ix, iy])
                return;

            if (mapDataLoader.TryGetEntityLoader(templateName, out IMapWorldEntityLoader entityLoader))
                entityLoader.Load(mapAssets, map, visited, ix, iy, templateName, flavor, gfxValue, world);
        }

        private IEntity LoadUnknownCodeCell(MapMapper worldBlockBuilder, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, IWorld world)
        {
            if (mapDataLoader.TryGetEntityLoader("Unknown", out IMapWorldEntityLoader entityLoader))
                return entityLoader.Load(worldBlockBuilder, map, visited, ix, iy, "Unknown", actionValue.ToString(), gfxValue, world);

            return null;
        }
    }
}
