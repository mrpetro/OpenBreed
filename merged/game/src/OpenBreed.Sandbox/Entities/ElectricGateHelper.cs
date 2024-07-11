using OpenBreed.Common.Tools;
using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Events;
using OpenBreed.Rendering.Interface;
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
using OpenBreed.Database.Interface;
using OpenBreed.Common;
using System.Globalization;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common.Interface;

namespace OpenBreed.Sandbox.Entities
{
    public class ElectricGateHelper
    {
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;
        private readonly IWorldMan worldMan;

        public ElectricGateHelper(
            IDataLoaderFactory dataLoaderFactory,
            IEntityFactory entityFactory,
            IWorldMan worldMan)
        {
            this.dataLoaderFactory = dataLoaderFactory;
            this.entityFactory = entityFactory;
            this.worldMan = worldMan;
        }

        public IEntity AddVertical(IWorld world, int x, int y, string level)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\Common\ElectricGateVertical")
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        public IEntity AddHorizontal(IWorld world, int x, int y, string level)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\Common\ElectricGateHorizontal")
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }
    }
}
