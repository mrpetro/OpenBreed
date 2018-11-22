using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Props
{
    public class PropSelectorVM : BaseViewModel
    {
        #region Private Fields

        private PropSetVM _currentPropSet;

        #endregion Private Fields

        #region Public Constructors

        public PropSelectorVM(PropSetsVM propSets)
        {
            PropSets = propSets;

            PropSets.PropertyChanged += PropSets_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public PropSetVM CurrentPropSet
        {
            get { return _currentPropSet; }
            set { SetProperty(ref _currentPropSet, value); }
        }

        public PropSetsVM PropSets { get; private set; }
        public int SelectedIndex { get; private set; }

        #endregion Public Properties

        #region Private Methods

        private void PropSets_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(PropSets.CurrentItem):
                    CurrentPropSet = PropSets.CurrentItem;
                    SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods

    }
}
