﻿using OpenBreed.Editor.VM.Base;
using System;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Database
{
    public class EntryEditorsViewItemVM : BaseViewModel
    {
        #region Private Fields

        private readonly Action<EntryEditorsViewItemVM> closeAction;

        #endregion Private Fields

        #region Public Constructors

        public EntryEditorsViewItemVM(EntryEditorVM entryEditor, Action<EntryEditorsViewItemVM> closeAction)
        {
            Editor = entryEditor ?? throw new ArgumentNullException(nameof(entryEditor));
            this.closeAction = closeAction ?? throw new ArgumentNullException(nameof(closeAction));

            CloseCommand = new Command(Close);
        }

        #endregion Public Constructors

        #region Public Properties

        public string Title => Editor.Title;

        public EntryEditorVM Editor { get; }

        public ICommand CloseCommand { get; }

        #endregion Public Properties

        #region Private Methods

        private void Close()
        {
            Editor.ClosingAction?.Invoke();
            closeAction.Invoke(this);
        }

        #endregion Private Methods
    }
}