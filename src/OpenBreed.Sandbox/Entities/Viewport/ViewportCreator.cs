using OpenBreed.Common.Tools;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
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

        public Entity CreateViewportEntity(string name, float x, float y, float width, float height, string templateName)
        {
            var viewport = entityFactory.Create($@"Vanilla\ABTA\Templates\Common\Viewports\{templateName}.xml")
                .SetParameter("startX", x)
                .SetParameter("startY", y)
                .Build();

            //var viewport = entityMan.Create();
            viewport.Tag = name;

            viewport.Get<ViewportComponent>().Size = new Vector2(width, height);

            return viewport;
        }

        #endregion Public Methods
    }
}