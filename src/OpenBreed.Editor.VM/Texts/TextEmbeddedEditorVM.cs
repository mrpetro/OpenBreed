using OpenBreed.Common.Data;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.Texts
{
    public class TextEmbeddedEditorVM : BaseViewModel, IEntryEditor<IDbText>, IEntryEditor<IDbTextEmbedded>
    {
        #region Private Fields

        private readonly TextsDataProvider textsDataProvider;

        private string text;

        private string dataRef;

        #endregion Private Fields

        #region Public Constructors

        public TextEmbeddedEditorVM(TextsDataProvider textsDataProvider)
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

        #endregion Public Properties

        #region Public Methods

        public virtual void UpdateEntry(IDbText entry) => UpdateEntry((IDbTextEmbedded)entry);

        public virtual void UpdateVM(IDbText entry) => UpdateVM((IDbTextEmbedded)entry);

        public void UpdateEntry(IDbTextEmbedded entry)
        {
            var model = textsDataProvider.GetText(entry.Id);
            model.Text = Text;
            entry.DataRef = DataRef;
        }

        public void UpdateVM(IDbTextEmbedded entry)
        {
            var model = textsDataProvider.GetText(entry.Id);

            if (model != null)
                Text = model.Text;

            DataRef = entry.DataRef;
        }

        #endregion Public Methods
    }
}