using OpenBreed.Common.Data;
using OpenBreed.Database.Interface.Items;

namespace OpenBreed.Editor.VM
{
    public abstract class ParentEntryEditor<E> : EntryEditorBaseVM<E> where E : IDbEntry
    {
        #region Private Fields

        private readonly DbEntrySubEditorFactory subEditorFactory;
        private IEntryEditor<E> subeditor;

        #endregion Private Fields

        #region Public Constructors

        public ParentEntryEditor(DbEntrySubEditorFactory subEditorFactory, IWorkspaceMan workspaceMan, IDialogProvider dialogProvider, string editorName) : base(workspaceMan, dialogProvider)
        {
            this.subEditorFactory = subEditorFactory;
            EditorName = editorName;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntryEditor<E> Subeditor
        {
            get { return subeditor; }
            private set { SetProperty(ref subeditor, value); }
        }

        public override string EditorName { get; }

        #endregion Public Properties

        #region Protected Methods

        protected override void UpdateVM(E source)
        {
            void SubmodelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                OnPropertyChanged(e.PropertyName);
            }

            if (Subeditor != null)
                Subeditor.PropertyChanged -= SubmodelPropertyChanged;

            Subeditor = subEditorFactory.Create(source);

            base.UpdateVM(source);
            Subeditor.UpdateVM(source);

            Subeditor.PropertyChanged += SubmodelPropertyChanged;
        }

        protected override void UpdateEntry(E target)
        {
            base.UpdateEntry(target);
            Subeditor.UpdateEntry(target);
        }

        #endregion Protected Methods
    }
}