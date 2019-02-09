using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Actions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorActionsToolVM : BaseViewModel
    {

        #region Private Fields

        private string _title;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorActionsToolVM(MapEditorVM parent)
        {
            Parent = parent;

            Items = new BindingList<ActionVM>();
            Items.ListChanged += (s, a) => OnPropertyChanged(nameof(Items));
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<ActionVM> Items { get; }

        public MapEditorVM Parent { get; }
        public int SelectedIndex { get; private set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Connect()
        {
            Parent.PropertyChanged += MapEditor_PropertyChanged;
        }

        #endregion Public Methods

        #region Private Methods

        private void MapEditor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var mapEditor = sender as MapEditorVM;

            switch (e.PropertyName)
            {
                case nameof(mapEditor.Editable):
                    OnCurrentMapChanged(mapEditor.Editable);
                    break;
                default:
                    break;
            }
        }

        private void OnCurrentMapChanged(MapVM map)
        {
            Items.UpdateAfter(() =>
            {
                Items.Clear();
                SelectedIndex = -1;

                if (map == null)
                    return;

                if(map.ActionSet == null)
                    return;

                map.ActionSet.Items.ForEach(item => Items.Add(item));
            });
        }

        #endregion Private Methods

    }
}
