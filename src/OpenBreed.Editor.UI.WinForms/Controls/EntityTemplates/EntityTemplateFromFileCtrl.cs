using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.DataSources;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.EntityTemplates;

namespace OpenBreed.Editor.UI.WinForms.Controls.EntityTemplates
{
    public partial class EntityTemplateFromFileCtrl : EntryEditorInnerCtrl
    {
        #region Private Fields

        private EntityTemplateFromFileEditorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public EntityTemplateFromFileCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(EntityTemplateFromFileEditorVM vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(vm));

            tbxFileDataRef.DataBindings.Add(nameof(tbxFileDataRef.Text), _vm, nameof(_vm.DataRef), false, DataSourceUpdateMode.OnPropertyChanged);

            tbxText.DataBindings.Add(nameof(tbxText.Text), _vm, nameof(_vm.EntityTemplate), false, DataSourceUpdateMode.OnPropertyChanged);
            tbxText.DataBindings.Add(nameof(tbxText.Enabled), _vm, nameof(_vm.EditEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Public Methods
    }
}
