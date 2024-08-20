using Microsoft.Extensions.Logging;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Tiles;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.Interface;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OpenTK.Windowing.Common;

namespace RendererTest.Wpf.App.VM
{
    public class SampleSceneVm : BaseViewModel
    {
        #region Private Fields

        private (int, int) cursorPos;

        #endregion Private Fields

        #region Public Constructors

        public SampleSceneVm(
            IFontMan fontMan)
        {
            InitFunc = OnInitialize;
            CursorAction = OnCursor;
        }

        #endregion Public Constructors

        #region Public Properties

        public Func<IGraphicsContext, IRenderView> InitFunc { get; }

        public Action<(int X, int Y)> CursorAction { get; }

        public Action<TimeSpan> RenderAction { get; }

        public Action<(int Width, int Height)> ResizeAction { get; }

        public ICommand RenderCommand { get; set; }

        #endregion Public Properties

        #region Private Methods

        private IRenderView OnInitialize(IGraphicsContext graphicsContext)
        {
            //renderFactory.Initialize();
            //var renderView = renderFactory.CreateView(graphicsContext, null, OnRender);
            //renderView.Reset();
            //renderView.Renderer = OnRender;
            //renderView.Resizer = OnResize;

            return null;
        }

        private void OnCursor((int X, int Y) tuple)
        {
            cursorPos = tuple;
        }

        private void OnStart()
        {
        }

        private void OnRender(IRenderView view, Matrix4 transform, Box2 viewBox, int depth, float dt)
        {
            view.Context.Primitives.DrawRectangle(view, new Box2(0, 0, 20, 30), Color4.Red, filled: true);

            //fontMan.Render(view, viewBox, dt, RenderTexts);
        }

        private void RenderTexts(IRenderView view, Box2 clipBox, float dt)
        {
            var cursorPos4 = new Vector4(
                cursorPos.Item1,
                cursorPos.Item2,
                0.0f,
                1.0f);

            cursorPos4 = view.GetScreenToWorldCoords(cursorPos4);

            var font = view.Context.Fonts.GetOSFont("ARIAL", 12);

            view.Context.Fonts.RenderStart(view, new Vector2(50, 200));
            view.Context.Fonts.RenderPart(view, font.Id, $"({cursorPos4.X},{cursorPos4.Y})", Vector2.Zero, Color4.Green, 100, clipBox);
            view.Context.Fonts.RenderEnd(view);
        }

        #endregion Private Methods
    }
}