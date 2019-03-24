using OpenBreed.Editor.VM.Base;
using System;

namespace OpenBreed.Editor.VM.Common
{
    public class EntryRefVM : BaseViewModel
    {

        #region Private Fields

        private readonly Type _entryType;
        private string _refId;

        #endregion Private Fields

        #region Public Constructors

        public EntryRefVM(Type type)
        {
            _entryType = type;
        }

        #endregion Public Constructors

        #region Public Properties

        public Action<EntryRefSelectorVM> OpenRefIdSelectorAction { get; set; }
        public string RefId
        {
            get { return _refId; }
            set { base.SetProperty(ref _refId, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void SelectActionSetId()
        {
            var refSelector = new EntryRefSelectorVM(_entryType);
            refSelector.CurrentEntryId = RefId;
            OpenRefIdSelectorAction?.Invoke(refSelector);

            if (refSelector.SelectedEntryId == null)
                return;

            RefId = refSelector.SelectedEntryId;
        }

        #endregion Public Methods

    }
}