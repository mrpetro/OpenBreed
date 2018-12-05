using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Maps.Tools;
using OpenBreed.Editor.VM;
using System.Drawing.Drawing2D;

namespace OpenBreed.Editor.UI.WinForms.Controls.Map
{
    public partial class MapBodyEditorCtrl : UserControl, IToolController
    {
        #region Private Fields

        private MapBodyEditorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public MapBodyEditorCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(MapBodyEditorVM vm)
        {
            _vm = vm;

            _vm.Root.ToolsMan.ClearTools();
            _vm.Root.ToolsMan.AddPassiveTool(new ScrollTool(_vm, this));
            _vm.Root.ToolsMan.AddPassiveTool(new ZoomTool(_vm, this));

            _vm.PropertyChanged += _vm_PropertyChanged;

            UpdateViewState();
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.Transformation):
                    Invalidate();
                    break;
                default:
                    break;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            e.Graphics.CompositingQuality = CompositingQuality.AssumeLinear;
            e.Graphics.CompositingMode = CompositingMode.SourceOver;
            e.Graphics.SmoothingMode = SmoothingMode.None;
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            _vm.DrawView(e.Graphics);

            base.OnPaint(e);
        }

        #endregion Protected Methods

        #region Private Methods

        private void SetMapState()
        {
            _vm.FitViewToBody(ClientRectangle.Width, ClientRectangle.Height);
        }

        private void SetNoMapState()
        {
            Invalidate();
        }

        private void UpdateViewState()
        {
            if (_vm.CurrentMapBody == null)
                SetNoMapState();
            else
                SetMapState();
        }

        #endregion Private Methods
    }
}
