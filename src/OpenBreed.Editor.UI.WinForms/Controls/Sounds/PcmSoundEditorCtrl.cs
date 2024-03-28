using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Editor.UI.WinForms.Helpers;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Sounds;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace OpenBreed.Editor.UI.WinForms.Controls.Sounds
{
    public partial class PcmSoundEditorCtrl : EntryEditorInnerCtrl
    {
        #region Private Fields

        private PcmSoundEditorVM vm;

        #endregion Private Fields

        #region Public Constructors

        public PcmSoundEditorCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(EntryEditorVM vm)
        {
            this.vm = vm as PcmSoundEditorVM ?? throw new InvalidOperationException(nameof(vm));

            var control = WpfHelper.CreateWpfControl<Wpf.Sounds.SoundFromPcmEditorCtrl>(this.vm);
            control.Dock = DockStyle.Fill;
            Panel.Controls.Add(control);
        }

        #endregion Public Methods
    }
}