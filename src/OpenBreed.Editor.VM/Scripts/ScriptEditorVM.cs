using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Scripts;
using System;

namespace OpenBreed.Editor.VM.Scripts
{
    public class ScriptEditorVM : EntryEditorBaseExVM<IScriptEntry>
    {
        #region Private Fields

        private IEntryEditor<IScriptEntry> subeditor;

        #endregion Private Fields

        #region Public Constructors

        public ScriptEditorVM(IRepository repository) : base(repository)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntryEditor<IScriptEntry> Subeditor
        {
            get { return subeditor; }
            private set { SetProperty(ref subeditor, value); }
        }

        public override string EditorName { get { return "Script Editor"; } }

        #endregion Public Properties

        #region Protected Methods

        protected override void UpdateVM(IScriptEntry source)
        {
            if (source is IScriptEmbeddedEntry)
                Subeditor = new ScriptEmbeddedEditorVM(this);
            else if (source is IScriptFromFileEntry)
                Subeditor = new ScriptFromFileEditorVM(this);
            else
                throw new NotImplementedException();

            base.UpdateVM(source);
            Subeditor.UpdateVM(source);
        }

        protected override void UpdateEntry(IScriptEntry target)
        {
            base.UpdateEntry(target);
            Subeditor.UpdateVM(target);
        }

        #endregion Protected Methods
    }
}