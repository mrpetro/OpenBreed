using OpenBreed.Common.Tools;
using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Xml;
using OpenTK.Graphics;

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
            var viewportTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>($@"Entities\Viewport\{templateName}.xml");
            var viewport = entityFactory.Create(viewportTemplate);

            //var viewport = entityMan.Create();
            viewport.Tag = name;

            viewport.Get<ViewportComponent>().Width = width;
            viewport.Get<ViewportComponent>().Height = height;
            viewport.Get<PositionComponent>().Value = new OpenTK.Vector2(x, y);

            return viewport;
        }

        #endregion Public Methods
    }
}