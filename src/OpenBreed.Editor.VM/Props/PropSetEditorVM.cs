using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Props
{
    public class PropSetEditorVM : BaseViewModel
    {

        #region Private Fields

        private PropSetVM _currentPropSet;
        private string _title;

        #endregion Private Fields

        #region Public Constructors

        public PropSetEditorVM(EditorVM root)
        {
            Root = root;

            PropertyChanged += This_PropertyChanged;
        }

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentPropSet):
                    if (CurrentPropSet != null)
                        Title = CurrentPropSet.Name;
                    else
                        Title = "No property set";

                    SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public PropSetVM CurrentPropSet
        {
            get { return _currentPropSet; }
            set { SetProperty(ref _currentPropSet, value); }
        }

        public EditorVM Root { get; }

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

        }

        internal void TryLoad(string name)
        {
            var propSet = Root.CreatePropSet();
            propSet.Load(name);
            CurrentPropSet = propSet;
        }

        #endregion Internal Methods

    }
}
