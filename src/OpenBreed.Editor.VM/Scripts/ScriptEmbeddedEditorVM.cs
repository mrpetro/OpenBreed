using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.Scripts
{
    public class ScriptEmbeddedEditorVM : BaseViewModel, IEntryEditor<IScriptEntry>
    {
        #region Private Fields

        private string _dataRef;

        private string _script;

        #endregion Private Fields

        #region Internal Constructors

        internal ScriptEmbeddedEditorVM(ScriptEditorVM parent)
        {
            Parent = parent;
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Script
        {
            get { return _script; }
            set { SetProperty(ref _script, value); }
        }

        public ScriptEditorVM Parent { get; }

        public string DataRef
        {
            get { return _dataRef; }
            set { SetProperty(ref _dataRef, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void UpdateVM(IScriptEntry entry)
        {
            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var model = dataProvider.Scripts.GetScript(entry.Id);

            if (model != null)
                Script = model.Script;

            DataRef = entry.DataRef;
        }

        public void UpdateEntry(IScriptEntry entry)
        {
            entry.DataRef = DataRef;
        }

        #endregion Public Methods
    }
}