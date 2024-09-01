using OpenBreed.Core.Interface.Managers;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.TileStamps
{
    public class TileStampRenderViewControl : RenderViewControlBase<IDbTileStamp>
    {
        #region Private Fields

        private readonly ITileStampDataLoader tileStampDataLoader;

        #endregion Private Fields

        #region Public Constructors

        public TileStampRenderViewControl(
            IEventsMan eventsMan,
            IRenderContext renderContext,
            ITileStampDataLoader tileStampDataLoader) : base(eventsMan, renderContext)
        {
            this.tileStampDataLoader = tileStampDataLoader;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(IDbTileStamp entry)
        {
            var tileStamp = tileStampDataLoader.Load(entry);
        }

        public override void Save(IDbTileStamp entry)
        {
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnRender(IRenderView view, Matrix4 transform, float dt)
        {
            view.Context.Primitives.DrawRectangle(view, new Box2(0, 0, 20, 30), Color4.Red, filled: true);
        }

        #endregion Protected Methods
    }
}