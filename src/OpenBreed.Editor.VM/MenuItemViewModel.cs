using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OpenBreed.Editor.VM
{
    public class MenuItemViewModel : ObservableObject
    {
        #region Public Properties

        public string Header { get; set; }

        public ICommand Command { get; set; }

        public ObservableCollection<MenuItemViewModel> Children { get; set; } = new();

        public bool IsVisible { get; set; } = true;

        public bool IsEnabled { get; set; } = true;

        public bool IsSeparator { get; set; }

        #endregion Public Properties
    }
}