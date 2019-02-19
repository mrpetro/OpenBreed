using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorToolsVM : BaseViewModel
    {

        #region Private Fields

        private MapEditorToolVM _currentTool;
        private int _currentToolIndex;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorToolsVM()
        {
            Items = new BindingList<MapEditorToolVM>();

            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public MapEditorToolVM CurrentTool
        {
            get { return _currentTool; }
            set { SetProperty(ref _currentTool, value); }
        }

        public int CurrentToolIndex
        {
            get { return _currentToolIndex; }
            set { SetProperty(ref _currentToolIndex, value); }
        }

        public BindingList<MapEditorToolVM> Items { get; }

        #endregion Public Properties

        #region Private Methods

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentToolIndex):
                    UpdateCurrentTool();
                    break;
                case nameof(CurrentTool):
                    UpdateCurrentToolIndex();
                    break;
                case nameof(Items):
                    CurrentTool = Items.FirstOrDefault();
                    break;
                default:
                    break;
            }
        }
        private void UpdateCurrentTool()
        {
            if (CurrentToolIndex == -1)
                CurrentTool = null;
            else
                CurrentTool = Items[CurrentToolIndex];
        }

        private void UpdateCurrentToolIndex()
        {
            if (CurrentTool == null)
                CurrentToolIndex = -1;
            else
                CurrentToolIndex = Items.IndexOf(CurrentTool);
        }

        #endregion Private Methods

    }
}
