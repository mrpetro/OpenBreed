using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.Texts
{
    public class TextEmbeddedEditorVM : TextEditorBaseVM<IDbTextEmbedded>
    {
        #region Private Fields

        private readonly TextsDataProvider textsDataProvider;

        private string text;

        private string dataRef;

        #endregion Private Fields

        #region Public Constructors

        public TextEmbeddedEditorVM(
            TextsDataProvider textsDataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            IControlFactory controlFactory) : base(workspaceMan, dialogProvider, controlFactory)
        {
            this.textsDataProvider = textsDataProvider;
        }

        #endregion Public Constructors

        #region Public Properties

        public string DataRef
        {
            get { return dataRef; }
            set { SetProperty(ref dataRef, value); }
        }

        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }

        public override string EditorName => "Embedded Text Editor";

        #endregion Public Properties

        #region Protected Methods

        protected override void UpdateEntry(IDbTextEmbedded entry)
        {
            var model = textsDataProvider.GetText(entry.Id);
            model.Text = Text;
            entry.DataRef = DataRef;
        }

        protected override void UpdateVM(IDbTextEmbedded entry)
        {
            var model = textsDataProvider.GetText(entry.Id);

            if (model != null)
                Text = model.Text;

            DataRef = entry.DataRef;
        }

        #endregion Protected Methods
    }
}