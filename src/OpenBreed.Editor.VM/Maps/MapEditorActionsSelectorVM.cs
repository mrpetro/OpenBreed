using OpenBreed.Common;
using OpenBreed.Editor.VM.Actions;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorActionsSelectorVM : BaseViewModel
    {
        #region Private Fields

        private int _selectedIndex;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorActionsSelectorVM(MapEditorActionsToolVM parent)
        {
            Parent = parent;

            Items = new BindingList<ActionVM>();
            Items.ListChanged += (s, a) => OnPropertyChanged(nameof(Items));

            Parent.PropertyChanged += Parent_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<ActionVM> Items { get; }

        public MapEditorActionsToolVM Parent { get; }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        #endregion Public Properties

        #region Private Methods

        private void OnActionSetChanged(ActionSetVM actionSet)
        {
            Items.UpdateAfter(() =>
            {
                Items.Clear();
                SelectedIndex = -1;

                if (actionSet == null)
                    return;

                actionSet.Items.ForEach(item => Items.Add(item));
            });
        }

        private void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Parent.ActionSet):
                    OnActionSetChanged(Parent.ActionSet);
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods

    }
}
