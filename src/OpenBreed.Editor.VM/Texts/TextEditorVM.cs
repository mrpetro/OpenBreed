using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Texts;
using System;

namespace OpenBreed.Editor.VM.Texts
{
    public class TextEditorVM : EntryEditorBaseExVM<ITextEntry>
    {
        #region Private Fields

        private IEntryEditor<ITextEntry> subeditor;


        #endregion Private Fields

        #region Public Constructors

        public TextEditorVM(IRepository repository) : base(repository)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntryEditor<ITextEntry> Subeditor
        {
            get { return subeditor; }
            private set { SetProperty(ref subeditor, value); }
        }

        public override string EditorName { get { return "Text Editor"; } }

        #endregion Public Properties

        #region Protected Methods

        protected override void UpdateVM(ITextEntry entry)
        {
            if (entry is ITextEmbeddedEntry)
                Subeditor = new TextEmbeddedEditorVM(this);
            else if (entry is ITextFromMapEntry)
                Subeditor = new TextFromMapEditorVM(this);
            else
                throw new NotImplementedException();

            base.UpdateVM(entry);
            Subeditor.UpdateVM(entry);
        }

        protected override void UpdateEntry(ITextEntry target)
        {
            base.UpdateEntry(target);
            Subeditor.UpdateEntry(target);
        }

        #endregion Protected Methods
    }
}