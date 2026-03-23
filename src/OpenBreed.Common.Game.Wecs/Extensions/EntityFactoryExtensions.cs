using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Rendering.Components;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Extensions
{
    public static class EntityFactoryExtensions
    {
        #region Public Methods

        public static IEntity CreatePlayerActor(this IEntityFactory entityFactory, string name, Vector2 pos)
        {
            var actor = entityFactory.CreateActor(name, pos);

            actor.Add(new EquipmentComponent(
                new[]{
                    new EquipmentSlot("Torso"),
                    new EquipmentSlot("Hands")
                }));
            actor.Add(new InventoryComponent(16));

            return actor;
        }

        public static IEntity CreateActor(this IEntityFactory entityFactory, string name, Vector2 pos)
        {
            var actor = entityFactory.Create($@"ABTA\Templates\Common\Actors\{name}")
                .SetParameter("startX", pos.X)
                .SetParameter("startY", pos.Y)
                .SetTag(name)
                .Build();

            return actor;
        }


        public static IEntity CreateCommentator(this IEntityFactory entityFactory)
        {
            return entityFactory.Create($@"ABTA\Templates\Common\GameCommentator")
                .SetTag("Commentator")
                .Build();
        }

        public static IEntity CreateViewport(this IEntityFactory entityFactory, string name, float x, float y, float width, float height, string templateName)
        {
            var viewport = entityFactory.Create($@"ABTA\Templates\Common\Viewports\{templateName}")
                .SetParameter("startX", x)
                .SetParameter("startY", y)
                .SetTag(name)
                .Build();

            viewport.Get<ViewportComponent>().Size = new Vector2(width, height);

            return viewport;
        }

        public static IEntity CreateCamera(this IEntityFactory entityFactory, string name, float x, float y, float width, float height)
        {
            var camera = entityFactory.Create(@"ABTA\Templates\Common\Camera")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .SetParameter("width", width)
                .SetParameter("height", height)
                .SetTag(name)
                .Build();

            return camera;
        }

        #endregion Public Methods
    }
}