using OpenBreed.Common.Data;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.Scripts
{
    public class ScriptEmbeddedEditorVM : BaseViewModel, IEntryEditor<IDbScript>, IEntryEditor<IDbScriptEmbedded>
    {
        #region Private Fields

        private readonly ScriptsDataProvider scriptsDataProvider;
        private string dataRef;

        private string script;

        #endregion Private Fields

        #region Public Constructors

        public ScriptEmbeddedEditorVM(ScriptsDataProvider scriptsDataProvider)
        {
            this.scriptsDataProvider = scriptsDataProvider;
        }

        #endregion Public Constructors

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

        public void UpdateVM(IDbScript entry) => UpdateVM((IDbScriptEmbedded)entry);

        public void UpdateEntry(IDbScript entry) => UpdateEntry((IDbScriptEmbedded)entry);

        public void UpdateEntry(IDbScriptEmbedded entry)
        {
            entry.DataRef = DataRef;
        }

        public void UpdateVM(IDbScriptEmbedded entry)
        {
            var model = scriptsDataProvider.GetScript(entry.Id);

            if (model != null)
                Script = model.Script;

            DataRef = entry.DataRef;
        }

        #endregion Public Methods
    }
}