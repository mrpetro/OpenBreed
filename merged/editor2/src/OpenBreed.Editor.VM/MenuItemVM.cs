using OpenBreed.Editor.VM.Base;
using System;

namespace OpenBreed.Editor.VM
{
    public class MenuItemVM : BaseViewModel
    {
        #region Private Fields

        private string name;

        private bool isChecked;

        #endregion Private Fields

        #region Public Constructors

        public MenuItemVM(string name, Action clickAction, bool isChecked)
        {
            Name = name;
            ClickAction = clickAction;
            IsChecked = isChecked;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public Action ClickAction { get; }

        public bool IsChecked
        {
            get { return isChecked; }
            set { SetProperty(ref isChecked, value); }
        }

        #endregion Public Properties
    }
}