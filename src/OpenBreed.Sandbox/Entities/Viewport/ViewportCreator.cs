using OpenBreed.Common.Tools;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Core;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Rendering.Components;
using OpenBreed.Wecs.Entities.Xml;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;

namespace OpenBreed.Sandbox.Entities.Viewport
{
    public class ViewportCreator
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IEntityFactory entityFactory;

        #endregion Private Fields

        #region Public Constructors

        public ViewportCreator(IEntityMan entityMan, IEntityFactory entityFactory)
        {
            this.entityMan = entityMan;
            this.entityFactory = entityFactory;
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntity CreateViewportEntity(string name, float x, float y, float width, float height, string templateName)
        {
            var viewport = entityFactory.Create($@"ABTA\Templates\Common\Viewports\{templateName}")
                .SetParameter("startX", x)
                .SetParameter("startY", y)
                .SetTag(name)
                .Build();

            //var viewport = entityMan.Create();
            //viewport.Tag = name;

            viewport.Get<ViewportComponent>().Size = new Vector2(width, height);

            return viewport;
        }

        #endregion Public Methods
    }
}