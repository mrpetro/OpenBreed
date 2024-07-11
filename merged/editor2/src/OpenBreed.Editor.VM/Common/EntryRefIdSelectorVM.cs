using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Common
{
    public class EntryRefIdSelectorVM : BaseViewModel
    {
        #region Private Fields

        private string _currentEntryId;
        private string _selectedEntryId;

        #endregion Private Fields

        public BindingList<string> Items { get; }

        public EntryRefIdSelectorVM(IWorkspaceMan workspaceMan, Type type)
        {
            Items = new BindingList<string>();

            var repository = workspaceMan.GetRepository(type);

            Items.UpdateAfter(() =>
            {
                repository.Entries.ForEach(item => Items.Add(item.Id));
            });
        }

        #region Public Properties

        public string CurrentEntryId
        {
            get { return _currentEntryId; }
            set { base.SetProperty(ref _currentEntryId, value); }
        }

        public string SelectedEntryId
        {
            get { return _selectedEntryId; }
            set { base.SetProperty(ref _selectedEntryId, value); }
        }

        public void Accept()
        {
            SelectedEntryId = CurrentEntryId;
        }

        #endregion Public Properties
    }
}
