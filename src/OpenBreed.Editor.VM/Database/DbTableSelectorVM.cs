using OpenBreed.Editor.VM.Base;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Database
{
    public class DbTableSelectorVM : BaseViewModel
    {
        #region Private Fields

        private string currentItem;

        private readonly EditorApplication application;

        #endregion Private Fields

        #region Internal Constructors

        internal DbTableSelectorVM(EditorApplication application)
        {
            this.application = application;

            Items = new BindingList<string>();
            Items.ListChanged += (s, a) => OnPropertyChanged(nameof(Items));

            UpdateWithDbTables();
        }

        #endregion Internal Constructors

        #region Public Properties

        public string CurrentItem
        {
            get { return currentItem; }
            set { SetProperty(ref currentItem, value); }
        }

        public BindingList<string> Items { get; }

        #endregion Public Properties

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(Items):
                    CurrentItem = Items.FirstOrDefault();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void UpdateWithDbTables()
        {
            Items.UpdateAfter(() =>
            {
                Items.Clear();

                foreach (var repository in application.UnitOfWork.Repositories)
                    Items.Add(repository.Name);
            });
        }

        #endregion Private Methods
    }
}