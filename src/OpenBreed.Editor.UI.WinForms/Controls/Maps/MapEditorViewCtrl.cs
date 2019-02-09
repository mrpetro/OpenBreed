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
using OpenBreed.Editor.VM.Tools;
using OpenBreed.Editor.VM;
using System.Drawing.Drawing2D;
using OpenBreed.Editor.VM.Renderer;

namespace OpenBreed.Editor.UI.WinForms.Controls.Maps
{
    public partial class MapEditorViewCtrl : UserControl, IToolView
    {
        #region Private Fields

        private ScrollTool _scrollTool;
        private ZoomTool _zoomTool;
        private MapEditorViewVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorViewCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(MapEditorViewVM vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(MapEditorViewVM));

            _vm.RefreshAction = this.Invalidate;

            _scrollTool = new ScrollTool(_vm, this);
            _scrollTool.Activate();
            _zoomTool = new ZoomTool(_vm, this);
            _zoomTool.Activate();

            Resize += (s,a) => _vm.Resize(this.ClientSize.Width, this.ClientSize.Height);

            _vm.PropertyChanged += _vm_PropertyChanged;
            
            UpdateViewState();
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.CurrentMapBody):
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
            if (_vm == null)
                return;

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
