using OpenBreed.Common;
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

        private const string PREFIX = @"Entities\Common\Environment";
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

        public void AddUnknownCell(World world, int x, int y, string name, string flavor = null)
        {
            var path = $@"{PREFIX}\{name}.xml";

            var pickable = entityFactory.Create(path)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("flavor", flavor)
                .Build();

            pickable.EnterWorld(world.Id);
        }

        internal void AddVoidCell(World world, int x, int y, int atlasId, int gfxValue)
        {
            var path = $@"{PREFIX}\Void.xml";

            var pickable = entityFactory.Create(path)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            pickable.PutTile(atlasId, gfxValue, 0, new OpenTK.Vector2(16 * x, 16 * y));

            pickable.EnterWorld(world.Id);
        }

        internal void AddObstacleCell(World world, int x, int y, int atlasId, int gfxValue)
        {
            var path = $@"{PREFIX}\Obstacle.xml";

            var pickable = entityFactory.Create(path)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            pickable.PutTile(atlasId, gfxValue, 0, new OpenTK.Vector2(16 * x, 16 * y));

            pickable.EnterWorld(world.Id);
        }

        #endregion Public Methods
    }
}
