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

        private ViewRenderer _renderer;
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

        private CursorButtons ToCursorButtons(MouseButtons buttons)
        {
            var cursorButtons = CursorButtons.None;

            if (buttons.HasFlag(MouseButtons.Left))
                cursorButtons |= CursorButtons.Left;
            if (buttons.HasFlag(MouseButtons.Right))
                cursorButtons |= CursorButtons.Right;
            if (buttons.HasFlag(MouseButtons.Middle))
                cursorButtons |= CursorButtons.Middle;

            return cursorButtons;
        }

        public void Initialize(MapEditorViewVM vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(MapEditorViewVM));

            _renderer = new ViewRenderer(_vm.Parent, _vm.RenderTarget);

            _vm.RefreshAction = this.Invalidate;

            _scrollTool = new ScrollTool(_vm, this);
            _scrollTool.Activate();
            _zoomTool = new ZoomTool(_vm, this);
            _zoomTool.Activate();

            MouseHover += (s, a) => _vm.Cursor.Hover();
            MouseLeave += (s, a) => vm.Cursor.Leave();
            MouseMove += (s, a) => vm.Cursor.Move(ToCursorButtons(a.Button), a.Location);
            MouseUp += (s, a) => vm.Cursor.Up(ToCursorButtons(a.Button), a.Location);
            MouseDown += (s, a) => vm.Cursor.Down(ToCursorButtons(a.Button), a.Location);

            Resize += (s, a) =>
            {
                _vm.Resize(this.ClientSize.Width, this.ClientSize.Height);
            };

            _vm.PropertyChanged += _vm_PropertyChanged;
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.Layout):
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

            _renderer.Render(_vm);

            _vm.Render(e.Graphics);

            base.OnPaint(e);
        }

        #endregion Protected Methods
    }
}
