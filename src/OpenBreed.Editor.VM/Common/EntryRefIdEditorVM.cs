using OpenBreed.Editor.VM.Base;
using System;

namespace OpenBreed.Editor.VM.Common
{
    public class EntryRefIdEditorVM : BaseViewModel
    {

        #region Private Fields

        private readonly Type _entryType;
        private string _refId;

        #endregion Private Fields

        #region Public Constructors

        public EntryRefIdEditorVM(Type type)
        {
            _entryType = type;
        }

        #endregion Public Constructors

        #region Public Properties

        public Action<string> RefIdSelected { get; set; }
        public Action<EntryRefIdSelectorVM> OpenRefIdSelectorAction { get; set; }

        public string RefId
        {
            get { return _refId; }
            set { base.SetProperty(ref _refId, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void SelectActionSetId()
        {
            var refSelector = new EntryRefIdSelectorVM(_entryType);
            refSelector.CurrentEntryId = RefId;
            OpenRefIdSelectorAction?.Invoke(refSelector);

            if (refSelector.SelectedEntryId == null)
                return;

            RefId = refSelector.SelectedEntryId;
            RefIdSelected?.Invoke(RefId);
        }

        #endregion Public Methods

    }
}