using OpenBreed.Audio.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Tools;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System.Collections.Generic;
using System.Globalization;

namespace OpenBreed.Sandbox.Entities.Pickable
{
    public class PickableHelper
    {
        #region Private Fields

        private const string PICKABLE_PREFIX = @"Vanilla\ABTA\Templates\Common\Pickables";
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;

        #endregion Private Fields

        #region Public Constructors

        public PickableHelper(IDataLoaderFactory dataLoaderFactory, IEntityFactory entityFactory)
        {
            this.dataLoaderFactory = dataLoaderFactory;
            this.entityFactory = entityFactory;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddItem(World world, int x, int y, string name, string level, int gfxValue, string flavor = null)
        {
            var path = $@"{PICKABLE_PREFIX}\{name}.xml";

            var pickable = entityFactory.Create(path)
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .SetParameter("flavor", flavor)
                .Build();

            pickable.EnterWorld(world.Id);
        }

        #endregion Public Methods
    }
}