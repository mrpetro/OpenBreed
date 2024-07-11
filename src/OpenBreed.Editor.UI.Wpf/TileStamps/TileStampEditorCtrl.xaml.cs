using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenBreed.Editor.UI.Wpf.TileStamps
{
    /// <summary>
    /// Interaction logic for TileStampEditorCtrl.xaml
    /// </summary>
    public partial class TileStampEditorCtrl : UserControl
    {
        public TileStampEditorCtrl()
        {
            InitializeComponent();

            var settings = new GLWpfControlSettings
            {
                MajorVersion = 4,
                MinorVersion = 0
            };
            OpenTkControl.Start(settings);


        }
        
        public Point GetMousePos() => this.PointToScreen(Mouse.GetPosition(this));

        private void OpenTkControl_OnRender(TimeSpan delta)
        {
            GL.ClearColor(Color4.Blue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);




        }
    }
}
