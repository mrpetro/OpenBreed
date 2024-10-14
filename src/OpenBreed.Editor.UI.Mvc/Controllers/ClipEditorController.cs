using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Editor.UI.Mvc.Models;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.Interface;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Editor.UI.Mvc.Views;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Rendering.Interface.Extensions;
using System.Drawing;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Editor.UI.Mvc.Extensions;

namespace OpenBreed.Editor.UI.Mvc.Controllers
{
    public class ClipEditorController : IController
    {
        #region Private Fields

        private const int cellSize = 16;
        private readonly EditorView view;
        private readonly IClipEditorModel model;

        #endregion Private Fields

        #region Public Constructors

        public ClipEditorController(
            IEventsMan eventsMan,
            EditorView view,
            IClipEditorModel model)
        {
            this.view = view;
            this.model = model;

            view.Rendering += OnRender;
            view.Reseting += OnReset;
            view.CursorDown += OnCursorDown;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Reset()
        {
            view.Reset();
        }

        #endregion Public Methods

        #region Private Methods

        private void OnReset(IRenderView view)
        {
            if (model.Track is null)
            {
                return;
            }

            var extent = model.GetExtent();

            var ratio = extent.Size.Y / extent.Size.X;


            var offset = new Vector2(-extent.Center.X, -extent.Center.Y);

            var scale = ratio * view.Box.Size.Y / extent.Size.Y;

            view.SetScale(scale);
            view.MoveTo(view.Box.HalfSize);
            view.MoveBy((Vector2i)(offset * scale));

        }

        private void OnRender(IRenderView view, Matrix4 transform, float dt)
        {
            view.EnableAlpha();
            //view.SetPalette(palette);
            RenderBorder(view);
            RenderAxes(view);
            RenderTimeScale(view);
            RenderTracks(view);
            view.DisableAlpha();
        }

        private void OnCursorDown(ViewCursorDownEvent e)
        {
            if (e.Key == CursorKeys.Left)
            {
                //var cursorPos = GetCellIndexCoords(e.View, e.Position) + new Vector4i(model.CenterX, model.CenterY, 0, 1);

                //model.PutTiles(cursorPos, CurrentTileAtlasId, CurrentTileSelection);
            }
            else if (e.Key == CursorKeys.Right)
            {
                //var cursorPos = GetCellIndexCoords(e.View, e.Position) + new Vector4i(model.CenterX, model.CenterY, 0, 1);

                //model.EraseTile(cursorPos);
            }
        }

        private void RenderBorder(IRenderView view)
        {
            //view.PushMatrix();
            //view.Translate(new Vector3(-model.CenterX * cellSize, -model.CenterY * cellSize, 0.0f));

            //var border = new Box2(0, 0, cellSize * model.Width, cellSize * model.Height);

            //view.Context.Primitives.DrawRectangle(view, border, new Color4(128, 128, 128, 128), filled: false);

            //view.PopMatrix();
        }

        private void RenderAxes(IRenderView view)
        {
            var worldBox = view.GetViewToWorldCoords(view.Box);

            view.Context.Primitives.DrawLine(view, new Vector2(worldBox.Min.X, 0), new Vector2(worldBox.Max.X, 0), Color4.Red);
            view.Context.Primitives.DrawLine(view, new Vector2(0, worldBox.Min.Y), new Vector2(0, worldBox.Max.Y), Color4.Green);
        }

        private void RenderTrack(IRenderView view, IDbAnimationTrack track)
        {
            switch (track)
            {
                case IDbAnimationTrack<int> intTrack:
                    RenderTrack(view, intTrack);
                    break;

                case IDbAnimationTrack<string> stringTrack:
                    RenderTrack(view, stringTrack);
                    break;

                default:
                    throw new NotImplementedException("Track type not implemented");
            }
        }

        private void RenderTrack(IRenderView view, IDbAnimationTrack<string> track)
        {
        }

        private void RenderTrack(IRenderView view, IDbAnimationTrack<int> track)
        {
            if (track.Frames.Count == 0)
            {
                return;
            }

            var points = track.Frames.Select(item => new Vector2(item.Time, item.Value)).ToArray();

            view.Context.Primitives.DrawPoints(view, points, Color4.Aqua, PointType.Rectangle, size: 10, ignoreScale: true);

            view.Context.Primitives.DrawLines(view, points, Color4.Aqua);
        }

        private void RenderTracks(IRenderView view)
        {
            var worldBox = view.GetViewToWorldCoords(view.Box);

            if (model.Track is null)
            {
                return;
            }

            RenderTrack(view, model.Track);

            view.Context.Primitives.DrawLine(view, new Vector2(worldBox.Min.X, 0), new Vector2(worldBox.Max.X, 0), Color4.Red);
            view.Context.Primitives.DrawLine(view, new Vector2(0, worldBox.Min.Y), new Vector2(0, worldBox.Max.Y), Color4.Green);
        }

        private void RenderTimeScale(IRenderView view)
        {
            var fontMan = view.Context.Fonts;

            var font = fontMan.GetOSFont("ARIAL", 8);
            var fontColor = Color4.Purple;

            var worldBox = view.GetViewToWorldCoords(view.Box);

            var maxX = worldBox.Max.X;

            if (maxX < 0.0f)
            {
                return;
            }

            var unit = 1.0f;

            var linesCount =  maxX / unit;

            var width = worldBox;

            var scaleUnit = 1.0f;

            var lineStepX = maxX / linesCount;

            lineStepX = (int)(lineStepX / scaleUnit) * scaleUnit;

            var viewLineStepX = lineStepX * view.GetScale();


            for (int i = 0; i < linesCount; i++)
            {
                var linePosX = lineStepX * i;

                view.Context.Primitives.DrawLine(view, new Vector2(linePosX, worldBox.Min.Y), new Vector2(linePosX, worldBox.Max.Y), Color4.Green);
            }

            //view.PushMatrix();

            //var scale = view.GetScale();

            //view.Scale(1.0f / scale);

            for (int i = 0; i < linesCount; i++)
            {
                var linePosX = lineStepX * i;

                var posText = linePosX.ToString();

                fontMan.RenderStart(view, new Vector2(linePosX, 0));
                fontMan.RenderPart(view, font.Id, posText, Vector2.Zero, fontColor, 100, worldBox, ignoreScale: true);
                fontMan.RenderEnd(view);
            }

            //view.PopMatrix();
        }

        #endregion Private Methods
    }
}