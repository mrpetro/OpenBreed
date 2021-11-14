using OpenBreed.Common.Data;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Model.Actions;
using OpenBreed.Model.Maps;
using System;
using System.ComponentModel;
using System.Drawing;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorActionsToolVM : MapEditorToolVM
    {
        #region Private Fields

        private string currentActionSetRef;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorActionsToolVM(MapEditorVM parent, IWorkspaceMan workspaceMan)
        {
            Parent = parent;
            RefIdEditor = new EntryRefIdEditorVM(workspaceMan, typeof(IDbActionSet));
            ActionsSelector = new MapEditorActionsSelectorVM(this);

            RefIdEditor.PropertyChanged += ActionEntryRef_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public MapLayoutModel Layout { get; private set; }
        public int LayerIndex { get; private set; }

        public MapEditorActionsSelectorVM ActionsSelector { get; }

        public string CurrentActionSetRef
        {
            get { return currentActionSetRef; }
            set { SetProperty(ref currentActionSetRef, value); }
        }

        public Action<string> ModelChangeAction { get; internal set; }
        public MapEditorVM Parent { get; }
        public EntryRefIdEditorVM RefIdEditor { get; }

        #endregion Public Properties

        #region Internal Properties

        internal ActionSetModel CurrentActionSet { get; private set; }

        #endregion Internal Properties

        #region Internal Methods

        internal override void OnCursor(MapViewCursorVM cursor)
        {
            if ((cursor.Action == CursorActions.Move || cursor.Action == CursorActions.Down) && cursor.Buttons.HasFlag(CursorButtons.Left))
                SetValue(cursor.WorldIndexCoords, ActionsSelector.SelectedIndex);
        }

        internal void SetValue(Point tileCoords, int value)
        {
            var oldValue = Layout.GetCellValue(LayerIndex, tileCoords.X, tileCoords.Y);

            if (oldValue == value)
                return;

            Layout.SetCellValue(LayerIndex, tileCoords.X, tileCoords.Y, value);

            Parent.IsModified = true;
        }

        internal void UpdateVM()
        {
            Layout = Parent.Layout;
            LayerIndex = Layout.GetLayerIndex(MapLayerType.Action);
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(CurrentActionSetRef):
                    RefIdEditor.RefId = CurrentActionSetRef;
                    UpdateModel();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void ActionEntryRef_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(RefIdEditor.RefId):
                    CurrentActionSetRef = RefIdEditor.RefId;
                    break;

                default:
                    break;
            }
        }

        private void UpdateModel()
        {
            ModelChangeAction?.Invoke(CurrentActionSetRef);
        }

        #endregion Private Methods
    }
}