using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OpenBreed.Editor.VM
{
    public class MenuItemVM : ObservableObject
    {
        #region Private Fields

        private bool isEnabled = true;

        #endregion Private Fields

        #region Public Properties

        public string Header { get; set; }

        public ICommand Command { get; set; }

        public ObservableCollection<MenuItemVM> Menus { get; set; } = new();

        public bool IsVisible { get; set; } = true;

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { SetProperty(ref isEnabled, value); }
        }

        public bool IsSeparator { get; set; }

        #endregion Public Properties
    }
}