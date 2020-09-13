using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.Texts
{
    public class TextEmbeddedEditorVM : BaseViewModel, IEntryEditor<ITextEntry>
    {
        #region Private Fields

        private string _text;

        private string _dataRef;

        #endregion Private Fields

        #region Public Constructors

        public TextEmbeddedEditorVM(TextEditorVM parent)
        {
            Parent = parent;
        }

        #endregion Public Constructors

        #region Public Properties

        public TextEditorVM Parent { get; }

        public string DataRef
        {
            get { return _dataRef; }
            set { SetProperty(ref _dataRef, value); }
        }

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public virtual void UpdateEntry(ITextEntry entry)
        {
            var model = Parent.DataProvider.Texts.GetText(entry.Id);
            model.Text = Text;
            entry.DataRef = DataRef;
        }

        public virtual void UpdateVM(ITextEntry entry)
        {
            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var model = dataProvider.Texts.GetText(entry.Id);

            if (model != null)
                Text = model.Text;

            DataRef = entry.DataRef;
        }

        #endregion Public Methods
    }
}