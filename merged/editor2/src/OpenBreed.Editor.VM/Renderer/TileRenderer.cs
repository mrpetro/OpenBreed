using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Model.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Renderer
{
    public class TileRenderer : RendererBase<TileModel>
    {
        #region Public Constructors

        public TileRenderer(IRenderTarget target) : base(target)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Render(TileModel renderable)
        {
        }

        #endregion Public Methods

        //private void RenderDefaultTile(RenderTarget renderTarget, int tileId, float x, float y, int tileSize)
        //{
        //    Font font = new Font("Arial", 5);

        //    var rectangle = new Rectangle((int)x, (int)y, tileSize, tileSize);

        //    Color c = Color.Black;
        //    Pen tileColor = new Pen(c);
        //    Brush brush = new SolidBrush(c);

        //    renderTarget.FillRectangle(brush, rectangle);

        //    c = Color.White;
        //    tileColor = new Pen(c);
        //    brush = new SolidBrush(c);

        //    renderTarget.DrawRectangle(tileColor, rectangle);
        //    renderTarget.DrawString(string.Format("{0,2:D2}", tileId / 100), font, brush, x + 2, y + 1);
        //    renderTarget.DrawString(string.Format("{0,2:D2}", tileId % 100), font, brush, x + 2, y + 7);
        //}

        //public void DrawTile(RenderTarget renderTarget, int tileId, float x, float y, int tileSize)
        //{
        //    if (Model.TileSet == null)
        //    {
        //        RenderDefaultTile(renderTarget, tileId, x, y, tileSize);
        //        return;
        //    }

        //    if (tileId >= Model.TileSet.Tiles.Count)
        //        return;

        //    var tileRect = Model.TileSet.Tiles[tileId].Rectangle;
        //    renderTarget.DrawImage(CurrentTilesBitmap, (int)x, (int)y, tileRect);
        //}
    }
}