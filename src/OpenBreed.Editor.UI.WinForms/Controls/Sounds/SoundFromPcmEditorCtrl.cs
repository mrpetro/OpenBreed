using OpenBreed.Editor.VM.Sounds;
using System.Windows.Forms;

namespace OpenBreed.Editor.UI.WinForms.Controls.Sounds
{
    public partial class SoundFromPcmEditorCtrl : UserControl
    {
        #region Private Fields

        private SoundFromPcmEditorVM vm;

        #endregion Private Fields

        #region Public Constructors

        public SoundFromPcmEditorCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(SoundFromPcmEditorVM vm)
        {
            this.vm = vm;

            btnPlaySound.Click += (s, a) => vm.Play();
        }

        #endregion Public Methods
    }
}