using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenBreed.Editor.UI.Mvc.Models;
using OpenBreed.Editor.UI.Mvc.Views;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Data;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK.Mathematics;
using System.IO.Pipes;
using System.Linq;

namespace OpenBreed.Editor.UI.Mvc.Controllers
{
    public class TileStampEditorController : IController
    {
        #region Private Fields

        private const int cellSize = 16;
        private readonly EditorView view;
        private readonly ITileStampEditorModel model;
        private readonly IPaletteMan paletteMan;
        private readonly PalettesDataProvider palettesDataProvider;
        private readonly ITileMan tileMan;
        private readonly ITileStampDataLoader tileStampDataLoader;
        private readonly ITileAtlasDataLoader tileAtlasDataLoader;
        private IPalette palette;
        private bool pendingReset;

        #endregion Private Fields

        #region Public Constructors

        public TileStampEditorController(
            IEventsMan eventsMan,
            EditorView view,
            ITileStampEditorModel model,
            IPaletteMan paletteMan,
            PalettesDataProvider palettesDataProvider,
            ITileMan tileMan,
            ITileStampDataLoader tileStampDataLoader,
            ITileAtlasDataLoader tileAtlasDataLoader
            )
        {
            this.view = view;
            this.model = model;
            this.paletteMan = paletteMan;
            this.palettesDataProvider = palettesDataProvider;
            this.tileMan = tileMan;
            this.tileStampDataLoader = tileStampDataLoader;
            this.tileAtlasDataLoader = tileAtlasDataLoader;

            view.Rendering += OnRender;
            view.CursorDown += OnCursorDown;

            LoadPalettes();
        }

        #endregion Public Constructors

        #region Public Properties

        public string CurrentTileAtlasId { get; set; }
        public TileSelection[] CurrentTileSelection { get; set; } = Array.Empty<TileSelection>();

        #endregion Public Properties

        #region Public Methods

        public void Reset()
        {
            view.Reset();
        }

        public Vector4i GetIndexPoint(Vector4 point)
        {
            var x = point.X / cellSize;
            var y = point.Y / cellSize;

            if (point.X < 0)
                x--;

            if (point.Y < 0)
                y--;

            return new Vector4i((int)x, (int)y, 0, 0);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnReset(IRenderView view)
        {
            view.MoveTo(view.Box.HalfSize);
            view.SetScale(2.0f);
        }

        private void OnRender(IRenderView view, float dt)
        {
            view.PushMatrix();

            if (pendingReset)
            {
                OnReset(view);
                pendingReset = false;
            }

            view.EnableAlpha();
            view.SetPalette(palette);
            view.PushMatrix();
            view.Translate(new Vector3(-model.CenterX * cellSize, -model.CenterY * cellSize, 0.0f));
            for (int i = 0; i < model.Cells.Count; i++)
            {
                var cell = model.Cells[i];

                view.PushMatrix();
                view.Translate(new Vector3(cell.X * cellSize, cell.Y * cellSize, 0.0f));

                var tileAtlas = tileMan.GetByName(cell.TsId);

                view.Context.Tiles.Render(view, tileAtlas.Id, cell.TsTi);

                view.PopMatrix();
            }

            view.PopMatrix();

            RenderCellHighlightByCursor(view);

            RenderBorder(view);

            RenderAxes(view);

            view.DisableAlpha();

            view.PopMatrix();
        }

        private void OnCursorDown(ViewCursorDownEvent e)
        {
            if (e.Key == CursorKey.Left)
            {
                var cursorPos = GetCellIndexCoords(e.View, e.Position) + new Vector4i(model.CenterX, model.CenterY, 0, 1);

                model.PutTiles(cursorPos, CurrentTileAtlasId, CurrentTileSelection);
            }
            else if (e.Key == CursorKey.Right)
            {
                var cursorPos = GetCellIndexCoords(e.View, e.Position) + new Vector4i(model.CenterX, model.CenterY, 0, 1);

                model.EraseTile(cursorPos);
            }
        }

        private Vector4i GetCellIndexCoords(IRenderView view, Vector2i viewPosition)
        {
            var worldPosition = view.ToWorldPoint(viewPosition);

            return GetIndexPoint(worldPosition);
        }

        private Vector4i GetCellCoords(IRenderView view, Vector2i viewPosition)
        {
            return GetCellIndexCoords(view, viewPosition) * cellSize;
        }

        private void RenderBorder(IRenderView view)
        {
            view.PushMatrix();
            view.Translate(new Vector3(-model.CenterX * cellSize, -model.CenterY * cellSize, 0.0f));

            var border = new Box2(0, 0, cellSize * model.Width, cellSize * model.Height);

            view.Context.Primitives.DrawRectangle(view, border, new Color4(128, 128, 128, 128), filled: false);

            view.PopMatrix();
        }

        private void RenderAxes(IRenderView view)
        {
            view.Context.Primitives.DrawLine(view, new Vector2(-200, 0), new Vector2(200, 0), Color4.Red);
            view.Context.Primitives.DrawLine(view, new Vector2(0, -200), new Vector2(0, 200), Color4.Green);
        }

        private void RenderCellHighlightByCursor(IRenderView view)
        {
            view.PushMatrix();
            var cellPos = GetCellCoords(view, this. view.CursorPosition);
            view.Translate(cellPos.X, cellPos.Y, 0.0f);

            if (!string.IsNullOrEmpty(CurrentTileAtlasId) && CurrentTileSelection is not null && CurrentTileSelection.Length > 0)
            {
                var tileAtlas = tileMan.GetByName(CurrentTileAtlasId);

                if (tileAtlas is null)
                {
                    tileAtlas = tileAtlasDataLoader.Load(CurrentTileAtlasId);
                }

                RenderCursorSelectedTiles(view, tileAtlas.Id);
            }

            view.Context.Primitives.DrawRectangle(view, new Box2(0, 0, cellSize, cellSize), new Color4(0, 0, 255, 64), filled: true);

            view.PopMatrix();
        }

        private void RenderCursorSelectedTiles(IRenderView view, int tileAtlasId)
        {
            for (int i = 0; i < CurrentTileSelection.Length; i++)
            {
                var tile = CurrentTileSelection[i];

                var cellPos = new Vector2i(tile.Position.X, tile.Position.Y) * cellSize;

                view.PushMatrix();
                view.Translate(cellPos.X, cellPos.Y, 0.0f);
                view.Context.Tiles.Render(view, tileAtlasId, tile.Index);
                view.PopMatrix();
            }
        }

        private void LoadPalettes()
        {
            var commonPaletteModel = palettesDataProvider.GetPalette("Palettes.COMMON");

            var builder = paletteMan.CreatePalette()
                .SetLength(256)
                .SetName("GamePalette")
                .SetColors(commonPaletteModel.Data.Select(color => color.ToColor4()).ToArray());

            var cb = commonPaletteModel[0];
            builder.SetColor(0, new Color4(cb.R / 255.0f, cb.G / 255.0f, cb.B / 255.0f, 0.0f));

            palette = builder.Build();
        }

        #endregion Private Methods
    }
}