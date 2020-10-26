using OpenBreed.Editor.VM.Actions;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Actions;
using System;
using System.ComponentModel;

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

        #region Internal Properties

        #endregion Internal Properties

        #region Private Methods

        private string currentActionSetRef;

        public string CurrentActionSetRef
        {
            get { return currentActionSetRef; }
            set { SetProperty(ref currentActionSetRef, value); }
        }

        private void OnActionSetChanged()
        {
            Items.UpdateAfter(() =>
            {
                Items.Clear();
                SelectedIndex = -1;

                if (Parent.Parent.ActionSet == null)
                    return;

                foreach (var actionModel in Parent.Parent.ActionSet.Items)
                {
                    Items.Add(new ActionVM(actionModel));
                }
            });
        }

        private void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Parent.CurrentActionSetRef):
                    OnActionSetChanged();
                    break;

                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}