using OpenBreed.Common;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Common
{


    public class RefSelectorVM : BaseViewModel
    {
        #region Private Fields

        private string _currentEntryId;
        private string _selectedEntryId;

        #endregion Private Fields

        public BindingList<string> Items { get; protected set; }

        protected RefSelectorVM()
        {
            Items = new BindingList<string>();
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

    public class RefSelectorVM<T> : RefSelectorVM where T : IEntry
    {
        public RefSelectorVM()
        {
            var repository = ServiceLocator.Instance.GetService<IUnitOfWork>().GetRepository<T>();

            Items.UpdateAfter(() =>
            {
                repository.Entries.ForEach(item => Items.Add(item.Id));
            });
        }
    }
}
