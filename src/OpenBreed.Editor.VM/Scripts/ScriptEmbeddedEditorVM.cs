using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.Scripts
{
    public class ScriptEmbeddedEditorVM : BaseViewModel, IEntryEditor<IScriptEntry>
    {
        #region Private Fields

        private string dataRef;

        private string script;

        #endregion Private Fields

        #region Internal Constructors

        internal ScriptEmbeddedEditorVM(ParentEntryEditor<IScriptEntry> parent)
        {
            Parent = parent;
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Script
        {
            get { return script; }
            set { SetProperty(ref script, value); }
        }

        public ParentEntryEditor<IScriptEntry> Parent { get; }

        public string DataRef
        {
            get { return dataRef; }
            set { SetProperty(ref dataRef, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void UpdateVM(IScriptEntry entry)
        {
            var model = Parent.DataProvider.Scripts.GetScript(entry.Id);

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