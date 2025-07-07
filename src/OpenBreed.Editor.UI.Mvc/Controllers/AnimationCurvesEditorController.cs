using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Editor.UI.Mvc.Models;
using OpenBreed.Rendering.Abstractions.Data;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions;
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
using OpenBreed.Rendering.Abstractions.Extensions;
using System.Drawing;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Editor.UI.Mvc.Extensions;
using OpenBreed.Common;
using OpenBreed.Core.Interface.Extensions;
using static System.Net.Mime.MediaTypeNames;
using System.Collections;
using static System.Formats.Asn1.AsnWriter;
using OpenBreed.Rendering.Abstractions.Renderers;

namespace OpenBreed.Editor.UI.Mvc.Controllers
{
    public class AnimationCurvesEditorController : IController
    {
        #region Private Fields

        private const int cellSize = 16;
        private readonly EditorView view;
        private readonly IClipEditorModel model;
        private readonly Color4 xUnitLineColor =  Color4.Green.SetAlpha(0.5f);
        private readonly Color4 yUnitLineColor = Color4.Red.SetAlpha(0.5f);
        private readonly Color4 xAxisLineColor = Color4.Red;
        private readonly Color4 yAxisLineColor = Color4.Green;

        #endregion Private Fields

        #region Public Constructors

        public AnimationCurvesEditorController(
            IEventsMan eventsMan,
            EditorView view,
            IClipEditorModel model)
        {
            this.view = view;
            this.model = model;

            view.Rendering += OnRender;
            view.CursorDown += OnCursorDown;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Reset()
        {
            view.SetScaleLimits(1.0f / (float)Math.Pow(2, 8), (float)Math.Pow(2, 8));
            view.Reset();
        }

        #endregion Public Methods

        #region Private Methods

        private void AutoCenter(IRenderView view)
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

        private void OnRender(IRenderView view, float dt)
        {
            view.PushMatrix();

            view.EnableAlpha();
            //view.SetPalette(palette);
            RenderBorder(view);

            RenderUnitGrid(view);
            RenderAxes(view);
            RenderTracks(view);
            view.DisableAlpha();

            view.PopMatrix();
        }

        private void OnCursorDown(ViewCursorDownEvent e)
        {
            if (e.Key == CursorKey.Left)
            {
                //var cursorPos = GetCellIndexCoords(e.View, e.Position) + new Vector4i(model.CenterX, model.CenterY, 0, 1);

                //model.PutTiles(cursorPos, CurrentTileAtlasId, CurrentTileSelection);
            }
            else if (e.Key == CursorKey.Right)
            {
                AutoCenter(e.View);

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
            var worldBox = view.ToWorldBox(view.Box);

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
            var worldBox = view.ToWorldBox(view.Box);

            if (model.Track is null)
            {
                return;
            }

            RenderTrack(view, model.Track);

            view.Context.Primitives.DrawLine(view, new Vector2(worldBox.Min.X, 0), new Vector2(worldBox.Max.X, 0), xAxisLineColor);
            view.Context.Primitives.DrawLine(view, new Vector2(0, worldBox.Min.Y), new Vector2(0, worldBox.Max.Y), yAxisLineColor);
        }

        private void RenderUnitGrid(IRenderView view)
        {

            var fontMan = view.Context.Fonts;

            var font = fontMan.GetOSFont("ARIAL", 6);
            var fontColor = Color4.Purple;

            var worldBox = view.ToWorldBox(view.Box);

            RenderUnitGridLines(view, worldBox);

            return;


            var minX = worldBox.Min.X;
            var minY = worldBox.Min.Y;

            var maxX = worldBox.Max.X;
            var maxY = worldBox.Max.Y;

            var minXUnit = 2.0f;
            var maxXUnit = 8.0f;

            var minYUnit = 1.0f;
            var maxYUnit = 8.0f;

            var scale = view.GetScale();

            var fontHeight = font.Height / scale;

            var scaleDivisonFactor = 4.0f;

            var boxWidth = view.Box.Size.X;
            var boxHeight = view.Box.Size.Y;

            //var fW = boxWidth / scale * scaleDivisonFactor;
            //var sx = (float)scale / fW;
            //var ssx = MathHelper.NextPowerOfTwo(sx);
            //var lineStepX = 1.0f / ssx;

            //lineStepX = MathHelper.Clamp(lineStepX, minXUnit, maxXUnit);
            var lineStepX = 4.0f;

            //var fH = boxHeight / scale * scaleDivisonFactor;
            //var sy = (float)scale / fH;
            //var ssy = MathHelper.NextPowerOfTwo(sy);
            //var lineStepY = 1.0f / ssy;

            //lineStepY = MathHelper.Clamp(lineStepY, minYUnit, maxYUnit);
            var lineStepY = 4.0f;

            var steps = new Vector2(lineStepX, lineStepY);

            var start = GetIndexPoint(worldBox.Min, steps) * steps;

           // var startX = ((int)(minX / lineStepX) + 1) * lineStepX;
            //var startY = ((int)(minY / lineStepY) + 1) * lineStepY;

            for (float linePosX = start.X; linePosX < maxX; linePosX += lineStepX)
            {
                var ps = new Vector2(linePosX, worldBox.Min.Y);
                var pe = new Vector2(linePosX, worldBox.Max.Y);

                view.Context.Primitives.DrawLine(view, ps, pe, Color4.Green);

                var posText = linePosX.ToString();

                view.PushMatrix();

                var tPos = new Vector2(linePosX, worldBox.Max.Y - fontHeight);

                var clipBox = worldBox.Translated(tPos * new Vector2(-1.0f, 1.0f));

                view.Translate(tPos);
                font.Draw(view, posText, fontColor, clipBox, ignoreScale: true);
                view.PopMatrix();
            }

            for (float linePosY = start.Y; linePosY < maxY; linePosY += lineStepY)
            {
                var ps = new Vector2(worldBox.Min.X, linePosY);
                var pe = new Vector2(worldBox.Max.X, linePosY);

                view.Context.Primitives.DrawLine(view, ps, pe, Color4.BurlyWood);

                var posText = linePosY.ToString();

                view.PushMatrix();

                var tPos = new Vector2(worldBox.Min.X, linePosY);

                var clipBox = worldBox.Translated(tPos * new Vector2(-1.0f, 1.0f));

                view.Translate(tPos);
                font.Draw(view, posText, fontColor, clipBox, ignoreScale: true);
                view.PopMatrix();
            }
        }

        public Vector2i GetIndexPoint(Vector2 point, Vector2 cellSize)
        {
            var x = point.X / cellSize.X;
            var y = point.Y / cellSize.Y;

            if (point.X > 0)
                x++;

            if (point.Y > 0)
                y++;

            return new Vector2i((int)x, (int)y);
        }

        public Box2i GetIndexRectangle(Box2 rect, Vector2 unit)
        {
            var min = GetIndexPoint(rect.Min, unit) - new Vector2i(1, 1);
            var max = GetIndexPoint(rect.Max, unit);

            return new Box2i((int)min.X, (int)min.Y, (int)max.X, (int)max.Y);
        }

        private float LimitLineUnit(float scale, float lineUnit)
        {
            var scaledXLineUnit = lineUnit * scale;

            if (scaledXLineUnit < 32)
            {
                while (scaledXLineUnit < 32)
                {
                    lineUnit++;
                    scaledXLineUnit = lineUnit * scale;

                }

                return lineUnit;
            }
            else if (scaledXLineUnit > 64)
            {
                //while (scaledXLineUnit > 64)
                //{
                //    xLineUnit--;
                //    scaledXLineUnit = xLineUnit * scale;

                //}
            }

            return lineUnit;
        }

        private void RenderUnitGridLines(IRenderView view, Box2 worldBox)
        {
            var scale = view.GetScale();

            var xLineUnit = 1.0f;
            var yLineUnit = 1.0f;

            var minXLineUnit = 2.0f;
            var maxXLineUnit = 8.0f;

            var minYLineUnit = 1.0f;
            var maxYLineUnit = 8.0f;


            xLineUnit = LimitLineUnit(scale, xLineUnit);
            yLineUnit = LimitLineUnit(scale, yLineUnit);

            var steps = new Vector2(xLineUnit, yLineUnit);
            var wBoxi = GetIndexRectangle(worldBox, steps);
            var drawStart = wBoxi.Min * steps;

            view.PushMatrix();

            view.Translate(new Vector2(drawStart.X, 0));

            for (int ix = wBoxi.Min.X; ix < wBoxi.Max.X; ix++)
            {
                var ps = new Vector2(0, worldBox.Min.Y);
                var pe = new Vector2(0, worldBox.Max.Y);
                view.Context.Primitives.DrawLine(view, ps, pe, xUnitLineColor);
                view.Translate(new Vector2(xLineUnit, 0));
            }

            view.PopMatrix();

            view.PushMatrix();

            view.Translate(new Vector2(0, drawStart.Y));

            for (int iy = wBoxi.Min.Y; iy < wBoxi.Max.Y; iy++)
            {
                var ps = new Vector2(worldBox.Min.X, 0);
                var pe = new Vector2(worldBox.Max.X, 0);

                view.Context.Primitives.DrawLine(view, ps, pe, yUnitLineColor);
                view.Translate(new Vector2(0, yLineUnit));
            }

            view.PopMatrix();


            //Draw unit texts

            var fontMan = view.Context.Fonts;
            var font = fontMan.GetOSFont("ARIAL", 10);
            var fontColor = Color4.Purple;
            var fontHeight = font.Height / scale;

            view.PushMatrix();

            view.Translate(new Vector2(drawStart.X - xLineUnit, worldBox.Max.Y - fontHeight));

            for (int ix = wBoxi.Min.X; ix < wBoxi.Max.X; ix++)
            {
                var linePosX = ix * xLineUnit;

                var posText = linePosX.ToString();

                var tPos = new Vector2(xLineUnit, 0.0f);

                var clipBox = worldBox.Translated(tPos * new Vector2(-1.0f, 1.0f));

                view.Translate(tPos);

                font.Draw(view, posText, fontColor, clipBox, ignoreScale: true);
            }

            view.PopMatrix();

            view.PushMatrix();

            view.Translate(new Vector2(worldBox.Min.X, drawStart.Y - yLineUnit));

            for (int iy = wBoxi.Min.Y; iy < wBoxi.Max.Y; iy++)
            {
                var linePosY = iy * yLineUnit;

                var posText = linePosY.ToString();

                var tPos = new Vector2(0.0f, yLineUnit);

                var clipBox = worldBox.Translated(tPos * new Vector2(-1.0f, 1.0f));

                view.Translate(tPos);
                font.Draw(view, posText, fontColor, clipBox, ignoreScale: true);
            }

            view.PopMatrix();
        }

        #endregion Private Methods
    }
}