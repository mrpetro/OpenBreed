using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Editor.UI.Mvc.Controllers;
using OpenBreed.Editor.UI.Mvc.Extensions;
using OpenBreed.Editor.UI.Mvc.Models;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Helpers;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions.Renderers;
using OpenBreed.Wecs.Entities;
using OpenTK.Mathematics;

namespace OpenBreed.Editor.UI.Mvc.Views
{
    public class AnimationCurvesEditorView : EditorView
    {
        #region Private Fields

        private readonly Color4 xUnitLineColor = Color4.Green.SetAlpha(0.5f);
        private readonly Color4 yUnitLineColor = Color4.Red.SetAlpha(0.5f);
        private readonly Color4 xAxisLineColor = Color4.Red;
        private readonly Color4 yAxisLineColor = Color4.Green;

        private readonly IAnimationEditorModel model;
        private readonly IAnimationSandbox animationSandbox;

        #endregion Private Fields

        #region Public Constructors

        public AnimationCurvesEditorView(
            IEventsMan eventsMan,
            IInteractionFactoryProvider guiFactoryProvider,
            IAnimationEditorModel model,
            IAnimationSandbox animationSandbox) : base(eventsMan, guiFactoryProvider)
        {
            this.model = model;
            this.animationSandbox = animationSandbox;
        }

        #endregion Public Constructors

        #region Public Methods

        public static string ToTime(TimeSpan timeSpan)
        {
            var result = timeSpan.ToString(@"m\:s\:ff");

            if (!result.StartsWith("0:"))
            {
                return result;
            }

            result = result.Remove(0, 2);

            return result;
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

        public override void Reset()
        {
            if (RenderView is null)
            {
                return;
            }

            SetScaleLimits(1.0f / (float)Math.Pow(2, 8), (float)Math.Pow(2, 12));
            base.Reset();
        }

        public void AutoCenter()
        {
            if (RenderView is null)
            {
                return;
            }

            if (model.CurrentTrack is null)
            {
                return;
            }

            var extent = model.GetExtent();

            var scaleX = 1.0f;
            var scaleY = 1.0f;

            if (extent.Size.X > 0)
            {
                scaleX = (float)RenderView.Box.Size.X / (float)extent.Size.X;
            }

            if (extent.Size.Y > 0)
            {
                scaleY = (float)RenderView.Box.Size.Y / (float)extent.Size.Y;
            }

            if (Math.Abs(scaleX) < 0.1)
            {
                scaleX = 0.1f;
            }

            if (Math.Abs(scaleY) < 0.1)
            {
                scaleY = 0.1f;
            }

            var offset = new Vector2(-extent.Center.X, -extent.Center.Y);

            RenderView.SetScale(scaleX, scaleY);
            RenderView.MoveTo(RenderView.Box.HalfSize);

            var sx = scaleX;
            var sy = scaleY;

            RenderView.MoveBy((Vector2i)(offset * new Vector2(sx, sy)));
        }

        public void InsertKeyFrame()
        {
        }

        #endregion Public Methods

        #region Protected Methods

        protected override Vector2 GetInteractionSnapCursorPosition(Vector2 worldPosition)
        {
            var stepX = (1, 50);
            var stepY = (1, 1);
            var snappedPosition = MyMathHelper.Snap(new Vector2(worldPosition.X, worldPosition.Y), stepX, stepY);

            return snappedPosition;
        }

        protected override void OnRender(IRenderView view, float dt)
        {
            view.PushMatrix();

            view.EnableAlpha();

            var extent = model.GetExtent();

            RenderBorder(view, extent);
            RenderUnitGrid(view, extent);
            RenderAxes(view, extent);
            RenderTracks(view, extent);

            if (model.Mode == AnimationCurvesEditorMode.SelectKeyFrames)
            {
                RenderHoveredKeyFrame(view, extent);

                RenderSelectedKeyFrames(view, extent);
            }

            RenderCurrentTimeLine(view);

            view.DisableAlpha();

            view.PopMatrix();
        }

        private void RenderHoveredKeyFrame(IRenderView view, MyExtentF extent)
        {
            if (model.HoveredKeyFrame is not null)
            {
                if (model.HoveredKeyFrame is ITrackKeyFrame<int> intKeyFrame)
                {
                    var point = new Vector2(intKeyFrame.Key, intKeyFrame.Value);
                    view.Context.Primitives.DrawPoint(view, point, Color4.Red, PointType.Circle, size: 15.0f, ignoreScale: true);
                }
            }
        }

        private void RenderSelectedKeyFrames(IRenderView view, MyExtentF extent)
        {
            foreach (var keyFrame in model.SelectedKeyFrames)
            {
                if (keyFrame is ITrackKeyFrame<int> intKeyFrame)
                {
                    var point = new Vector2(intKeyFrame.Key, intKeyFrame.Value);
                    view.Context.Primitives.DrawPoint(view, point, Color4.Red, PointType.CircleFilled, size: 10.0f, ignoreScale: true);
                }
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void RenderCurrentTimeLine(IRenderView view)
        {
            var currentTime = animationSandbox.CurrentTime;

            var worldBox = view.ToWorldBox(view.Box);

            view.Context.Primitives.DrawLine(view, new Vector2(currentTime, worldBox.Min.Y), new Vector2(currentTime, worldBox.Max.Y), Color4.White);
        }

        private void RenderBorder(IRenderView view, MyExtentF extent)
        {
            view.PushMatrix();

            var border = new Box2(0, extent.Min.Y, model.ClipLength, extent.Max.Y);

            view.Context.Primitives.DrawRectangle(view, border, new Color4(64, 64, 64, 64), filled: true);

            view.PopMatrix();
        }

        private void RenderAxes(IRenderView view, MyExtentF extent)
        {
            var worldBox = view.ToWorldBox(view.Box);

            view.Context.Primitives.DrawLine(view, new Vector2(worldBox.Min.X, 0), new Vector2(worldBox.Max.X, 0), Color4.Red);
            view.Context.Primitives.DrawLine(view, new Vector2(0, worldBox.Min.Y), new Vector2(0, worldBox.Max.Y), Color4.Green);
        }

        private void RenderTrack(IRenderView view, IReadOnlyTrack<IEntity> track)
        {
            switch (track)
            {
                case IReadOnlyTrack<IEntity, int> intTrack:
                    RenderTrack(view, intTrack);
                    break;

                case IReadOnlyTrack<IEntity, string> stringTrack:
                    RenderTrack(view, stringTrack);
                    break;

                default:
                    throw new NotImplementedException("Track type not implemented");
            }
        }

        private void RenderTrack(IRenderView view, IReadOnlyTrack<IEntity, string> track)
        {
        }

        private void RenderTrack(IRenderView view, IReadOnlyTrack<IEntity, int> track)
        {
            if (track.Frames.Count == 0)
            {
                return;
            }

            var points = track.Frames.Select(item => new Vector2(item.Key, item.Value)).ToArray();

            view.Context.Primitives.DrawPoints(view, points, Color4.Aqua, PointType.Rectangle, size: 10, ignoreScale: true);

            view.Context.Primitives.DrawLines(view, points, Color4.Aqua);
        }

        private void RenderTracks(IRenderView view, MyExtentF extent)
        {
            var worldBox = view.ToWorldBox(view.Box);

            if (model.CurrentTrack is null)
            {
                return;
            }

            RenderTrack(view, model.CurrentTrack);
        }

        private void RenderUnitGrid(IRenderView view, MyExtentF extent)
        {
            var fontMan = view.Context.ServiceProvider.GetRequiredService<IFontMan>();

            var font = fontMan.GetOSFont("ARIAL", 6);
            var fontColor = Color4.Purple;

            var worldBox = view.ToWorldBox(view.Box);

            RenderUnitGridLines(view, worldBox);
        }

        private void RenderTimeLabel(IRenderView view, float time, IFontAtlas font, Color4 fontColor, Box2 worldBox)
        {
            var timeInSeconds = TimeSpan.FromSeconds(time);
            var timeText = ToTime(timeInSeconds);

            font.Draw(view, timeText, fontColor, worldBox, ignoreScale: true);
        }

        private void RenderValueLabel(IRenderView view, float value, IFontAtlas font, Color4 fontColor, Box2 worldBox)
        {
            var valueText = value.ToString();

            font.Draw(view, valueText, fontColor, worldBox, ignoreScale: true);
        }

        private void RenderUnitGridLines(IRenderView view, Box2 worldBox)
        {
            var scale = view.GetScale();

            var xLineUnit = 0.1f;
            var yLineUnit = 1.0f;

            var x = scale.X % xLineUnit;
            var y = scale.Y % yLineUnit;

            scale = scale - new Vector2(x, y);

            //xLineUnit = LimitLineUnit(scale.X, xLineUnit);
            //yLineUnit = LimitLineUnit(scale.Y, yLineUnit);

            var fontMan = view.Context.ServiceProvider.GetRequiredService<IFontMan>();

            var font = fontMan.GetOSFont("ARIAL", 8);
            var fontColor = Color4.Purple;

            var labelWidth = font.GetWidth("00:00.000") / scale.X;
            var labelHeight = font.Height / scale.Y;

            var labelIndexWidth = ((int)(labelWidth / xLineUnit) + 1);
            var labelIndexHeight = ((int)(labelHeight / yLineUnit) + 1);

            var steps = new Vector2(xLineUnit, yLineUnit);
            var wBoxi = GetIndexRectangle(worldBox, steps);
            wBoxi.Min = new Vector2i(Math.Max(wBoxi.Min.X, 0), wBoxi.Min.Y);

            var drawStart = wBoxi.Min * steps;

            view.PushMatrix();

            view.Translate(new Vector2(drawStart.X, 0));

            for (int ix = wBoxi.Min.X; ix < wBoxi.Max.X; ix++)
            {
                var labelIndexWidthMod = ix % labelIndexWidth;

                var time = ix * xLineUnit;

                if (labelIndexWidthMod == 0)
                {
                    var ps = new Vector2(0, worldBox.Min.Y);
                    var pe = new Vector2(0, worldBox.Max.Y);
                    view.Context.Primitives.DrawLine(view, ps, pe, xUnitLineColor);
                }

                view.Translate(new Vector2(xLineUnit, 0));
            }

            view.PopMatrix();

            view.PushMatrix();

            view.Translate(new Vector2(0, drawStart.Y));

            for (int iy = wBoxi.Min.Y; iy < wBoxi.Max.Y; iy++)
            {
                var labelIndexHeightMod = iy % labelIndexHeight;

                var value = iy * yLineUnit;

                if (labelIndexHeightMod == 0)
                {
                    var ps = new Vector2(worldBox.Min.X, 0);
                    var pe = new Vector2(worldBox.Max.X, 0);

                    view.Context.Primitives.DrawLine(view, ps, pe, yUnitLineColor);
                }

                view.Translate(new Vector2(0, yLineUnit));
            }

            view.PopMatrix();

            //Draw unit texts

            view.PushMatrix();

            view.Translate(new Vector2(drawStart.X, worldBox.Max.Y - labelHeight));

            for (int ix = wBoxi.Min.X; ix < wBoxi.Max.X; ix++)
            {
                var labelIndexWidthMod = ix % labelIndexWidth;

                var time = ix * xLineUnit;

                var tPos = new Vector2(xLineUnit, 0.0f);

                if (labelIndexWidthMod == 0)
                {
                    //var clipBox = worldBox.Translated(tPos * new Vector2(-1.0f, 1.0f));
                    RenderTimeLabel(view, time, font, fontColor, worldBox);
                }

                view.Translate(tPos);
            }

            view.PopMatrix();

            view.PushMatrix();

            view.Translate(new Vector2(worldBox.Min.X, drawStart.Y));

            for (int iy = wBoxi.Min.Y; iy < wBoxi.Max.Y; iy++)
            {
                var labelIndexHeightMod = iy % labelIndexHeight;

                var value = iy * yLineUnit;

                var tPos = new Vector2(0.0f, yLineUnit);

                if (labelIndexHeightMod == 0)
                {
                    //var clipBox = worldBox.Translated(tPos * new Vector2(-1.0f, 1.0f));
                    RenderValueLabel(view, value, font, fontColor, worldBox);
                }

                view.Translate(tPos);
            }

            view.PopMatrix();
        }

        private float LimitLineUnit(float scale, float lineUnit)
        {
            //var scaledXLineUnit = lineUnit * scale;

            //if (scaledXLineUnit < 10)
            //{
            //    while (scaledXLineUnit < 10)
            //    {
            //        lineUnit++;
            //        scaledXLineUnit = lineUnit * scale;
            //    }

            //    return lineUnit;
            //}

            return lineUnit;
        }

        #endregion Private Methods
    }
}