﻿using OpenBreed.Editor.VM.Base;
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

        #region Public Constructors

        public MapEditorActionsToolVM(MapEditorVM parent)
        {
            Parent = parent;

            ActionsMan = new MapEditorActionsManVM(this);
            ActionsSelector = new MapEditorActionsSelectorVM(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public MapEditorActionsManVM ActionsMan { get; }
        public MapEditorActionsSelectorVM ActionsSelector { get; }
        public MapEditorVM Parent { get; }

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
            UpdateActionsMan(map);
            UpdateActionsSelector(map);
        }

        private void UpdateActionsMan(MapVM map)
        {
            ActionsMan.ActionSetId = null;

            if (map == null)
                return;

            if (map.ActionSet == null)
                return;

            ActionsMan.ActionSetId = map.ActionSet.Id;
        }

        private void UpdateActionsSelector(MapVM map)
        {
            ActionsSelector.Items.UpdateAfter(() =>
            {
                ActionsSelector.Items.Clear();
                ActionsSelector.SelectedIndex = -1;

                if (map == null)
                    return;

                if (map.ActionSet == null)
                    return;

                map.ActionSet.Items.ForEach(item => ActionsSelector.Items.Add(item));
            });
        }

        #endregion Private Methods
    }
}
