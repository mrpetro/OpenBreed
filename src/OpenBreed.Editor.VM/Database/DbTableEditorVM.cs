using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database.Items;
using OpenBreed.Editor.VM.Database.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    public class DbTableEditorVM : BaseViewModel
    {
        //public MyIcommand CancelCommand { get; private set; }


        private DbEntryVM _currentItem;

        #region Internal Constructors

        internal DbTableEditorVM()
        {

            Items = new BindingList<Items.DbEntryVM>();
            Items.ListChanged += (s, a) => OnPropertyChanged(nameof(Items));
        }

        #endregion Internal Constructors

        #region Public Properties

        public BindingList<DbEntryVM> Items { get; }

        public DbEntryVM CurrentItem
        {
            get { return _currentItem; }
            set { SetProperty(ref _currentItem, value); }
        }

        public void OpenEntity(DbEntryVM item)
        {
            var editor = ServiceLocator.Instance.GetService<EditorVM>().DbEditor.OpenEditor(item.GetType());
            editor.OpenEntry(item.Name);

            //item.Open();
        }

        #endregion Public Properties


    }
}