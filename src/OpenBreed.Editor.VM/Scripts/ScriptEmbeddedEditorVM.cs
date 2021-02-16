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
        private readonly ScriptsDataProvider scriptsDataProvider;

        #endregion Private Fields

        #region Internal Constructors

        internal ScriptEmbeddedEditorVM(ScriptsDataProvider scriptsDataProvider)
        {
            this.scriptsDataProvider = scriptsDataProvider;
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Script
        {
            get { return script; }
            set { SetProperty(ref script, value); }
        }

        public string DataRef
        {
            get { return dataRef; }
            set { SetProperty(ref dataRef, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void UpdateVM(IScriptEntry entry)
        {
            var model = scriptsDataProvider.GetScript(entry.Id);

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