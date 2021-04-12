using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenTK.Graphics;

namespace OpenBreed.Game
{
    public class ViewportCreator
    {
        #region Private Fields

        private readonly ICore core;
        private readonly IEntityMan entityMan;

        #endregion Private Fields

        #region Public Constructors

        public ViewportCreator(ICore core, IEntityMan entityMan)
        {
            this.core = core;
            this.entityMan = entityMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public Entity CreateViewportEntity(string name, float x, float y, float width, float height, bool drawBackground, bool clipping = true)
        {
            var viewport = entityMan.Create();
            viewport.Tag = name;

            var vpcBuilder = ViewportComponentBuilderEx.New();
            vpcBuilder.SetSize(width, height);
            vpcBuilder.SetDrawBorderFlag(true);
            vpcBuilder.SetDrawBackgroundFlag(drawBackground);
            vpcBuilder.SetClippingFlag(clipping);
            vpcBuilder.SetBackgroundColor(Color4.Black);

            viewport.Add(vpcBuilder.Build());
            viewport.Add(PositionComponent.Create(x, y));

            return viewport;
        }

        #endregion Public Methods
    }
}