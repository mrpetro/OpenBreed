using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Editor.VM.Maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Renderer
{
    public class TilesSelectorSelectionRenderer : RendererBase<MapEditorTilesSelectorVM>
    {
        #region Private Fields

        private readonly MapEditorTilesToolVM tilesTool;
        private readonly IDrawingFactory drawingFactory;

        #endregion Private Fields

        #region Public Constructors

        public TilesSelectorSelectionRenderer(MapEditorTilesToolVM tilesTool, IRenderTarget target, IDrawingFactory drawingFactory) : base(target)
        {
            this.tilesTool = tilesTool;
            this.drawingFactory = drawingFactory;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Render(MapEditorTilesSelectorVM renderable)
        {
            if (renderable == null)
                return;

            var selectedPen = drawingFactory.CreatePen(MyColor.LightGreen);
            var selectPen = drawingFactory.CreatePen(MyColor.LightBlue);
            var deselectPen = drawingFactory.CreatePen(MyColor.Red);

            for (int index = 0; index < renderable.SelectedIndexes.Count; index++)
            {
                var rectangle = renderable.CurrentTileSet.Tiles[renderable.SelectedIndexes[index]].Rectangle;
                Target.DrawRectangle(selectedPen, rectangle);
            }

            if (renderable.SelectMode == SelectModeEnum.Select)
                Target.DrawRectangle(selectPen, renderable.SelectionRectangle.GetRectangle(renderable.CurrentTileSet.TileSize));
            else if (renderable.SelectMode == SelectModeEnum.Deselect)
                Target.DrawRectangle(deselectPen, renderable.SelectionRectangle.GetRectangle(renderable.CurrentTileSet.TileSize));
        }

        #endregion Public Methods
    }
}
