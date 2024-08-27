using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace OpenBreed.Editor.UI.Wpf
{
    public class GLWpfControlEx : GLWpfControl
    {
        #region Public Fields

        public static readonly DependencyProperty InitFuncProperty =
            DependencyProperty.Register(nameof(InitFunc), typeof(Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext>), typeof(GLWpfControlEx));

        #endregion Public Fields

        #region Private Fields

        private IRenderContext renderContext;

        #endregion Private Fields

        #region Public Constructors

        public GLWpfControlEx()
        {
            var mainSettings = new GLWpfControlSettings { MajorVersion = 4, MinorVersion = 0 };

            Start(mainSettings);

            Render += GLWpfControlEx_Init;

            Cursor = Cursors.None;
        }

        #endregion Public Constructors

        #region Public Properties

        [Bindable(true)]
        public Func<IGraphicsContext, HostCoordinateSystemConverter , IRenderContext> InitFunc
        {
            get { return (Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext>)GetValue(InitFuncProperty); }
            set { SetValue(InitFuncProperty, value); }
        }

        #endregion Public Properties

        #region Private Methods

        private void GLWpfControlEx_Init(TimeSpan obj)
        {
            if (Context is null)
            {
                return;
            }

            if (InitFunc is null)
            {
                return;
            }

            renderContext = InitFunc.Invoke(Context, GetRenderContextPosition);

            renderContext.Resize((int)ActualWidth, (int)ActualHeight);

            Render -= GLWpfControlEx_Init;
            Render += GLWpfControlEx_Render;
            SizeChanged += GLWpfControlEx_SizeChanged;
            MouseMove += GLWpfControlEx_MouseMove;
            MouseDown += GLWpfControlEx_MouseDown;
            MouseEnter += GLWpfControlEx_MouseEnter;
            MouseLeave += GLWpfControlEx_MouseLeave; ;
            MouseUp += GLWpfControlEx_MouseUp;
            MouseWheel += GLWpfControlEx_MouseWheel;
        }

        private void GLWpfControlEx_MouseLeave(object sender, MouseEventArgs e)
        {
            var cursorPosition = FromPoint(e.GetPosition(this));
            renderContext.CursorLeave(0, cursorPosition);
        }

        private void GLWpfControlEx_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var cursorPosition = FromPoint(e.GetPosition(this));
            renderContext.CursorUp(0, cursorPosition, (CursorKeys)e.ChangedButton);
        }

        private void GLWpfControlEx_MouseEnter(object sender, MouseEventArgs e)
        {
            var cursorPosition = FromPoint(e.GetPosition(this));
            renderContext.CursorEnter(0, cursorPosition);
        }

        private void GLWpfControlEx_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var cursorPosition = FromPoint(e.GetPosition(this));
            renderContext.CursorDown(0, cursorPosition, (CursorKeys)e.ChangedButton);
        }

        private void GLWpfControlEx_MouseMove(object sender, MouseEventArgs e)
        {
            var cursorPosition = FromPoint(e.GetPosition(this));
            renderContext.CursorMove(0, cursorPosition);
        }

        private void GLWpfControlEx_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var cursorPosition = FromPoint(e.GetPosition(this));
            renderContext.CursorWheel(0, cursorPosition, e.Delta);
        }

        private void GLWpfControlEx_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            renderContext.Resize((int)e.NewSize.Width, (int)e.NewSize.Height);
        }

        private void GLWpfControlEx_Render(TimeSpan delta)
        {
            renderContext.Render((float)delta.TotalMilliseconds);
        }

        private Vector2i FromPoint(Point point)
        {
            return new Vector2i((int)point.X, (int)point.Y);
        }

        private Vector2i GetRenderContextPosition(Vector2i point)
        {
            var pointV = new Vector4(point.X, point.Y, 0.0f, 1.0f);

            var translateTranform = Matrix4.CreateTranslation(0.0f, (float)ActualHeight, 0.0f);
            var flipYTransform = Matrix4.CreateScale(1.0f, -1.0f, 1.0f);

            var matT = flipYTransform * translateTranform;
            pointV *= matT;

            return new Vector2i((int)pointV.X, (int)pointV.Y);
        }

        #endregion Private Methods
    }
}