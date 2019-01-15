using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Props;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Levels
{
    public class LevelPropSelectorVM : BaseViewModel
    {

        #region Private Fields

        private PropSetVM _currentItem;

        private string _title;

        #endregion Private Fields

        #region Public Constructors

        public LevelPropSelectorVM(LevelEditorVM parent)
        {
            Parent = parent;

            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public PropSetVM CurrentItem
        {
            get { return _currentItem; }
            set { SetProperty(ref _currentItem, value); }
        }

        public LevelEditorVM Parent { get; }
        public int SelectedIndex { get; private set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion Public Properties

        #region Internal Methods

        internal void Connect()
        {
            //Parent.Root.PropertyChanged += Root_PropertyChanged;
        }

        #endregion Internal Methods

        #region Private Methods

        private void Root_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Parent.CurrentLevel.PropSet):
                    CurrentItem = Parent.CurrentLevel.PropSet;
                    break;
                default:
                    break;
            }
        }
        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentItem):
                    if (CurrentItem != null)
                        Title = CurrentItem.Name;
                    else
                        Title = "No property set";

                    SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods

    }
}
