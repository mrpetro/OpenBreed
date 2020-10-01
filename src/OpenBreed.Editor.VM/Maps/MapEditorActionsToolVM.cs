using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Editor.VM.Maps.Layers;
using OpenBreed.Model.Actions;
using System;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorActionsToolVM : MapEditorToolVM
    {
        #region Private Fields

        private string currentActionSetRef;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorActionsToolVM(MapEditorVM parent)
        {
            Parent = parent;
            RefIdEditor = new EntryRefIdEditorVM(typeof(IActionSetEntry));
            ActionsSelector = new MapEditorActionsSelectorVM(this);

            RefIdEditor.PropertyChanged += ActionEntryRef_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public string CurrentActionSetRef
        {
            get { return currentActionSetRef; }
            set { SetProperty(ref currentActionSetRef, value); }
        }

        public EntryRefIdEditorVM RefIdEditor { get; }
        public MapEditorActionsSelectorVM ActionsSelector { get; }
        public MapEditorVM Parent { get; }
        public Action<string> ModelChangeAction { get; internal set; }

        #endregion Public Properties

        #region Internal Properties

        internal ActionSetModel CurrentActionSet { get; private set; }

        #endregion Internal Properties

        #region Internal Methods

        internal override void OnCursor(MapViewCursorVM cursor)
        {
            if ((cursor.Action == CursorActions.Move || cursor.Action == CursorActions.Down) && cursor.Buttons.HasFlag(CursorButtons.Left))
            {
                var actionLayer = Parent.Layout.Layers.OfType<MapLayerActionVM>().FirstOrDefault();

                var actionCode = ActionsSelector.SelectedIndex;

                actionLayer.SetCell(cursor.WorldIndexCoords.X, cursor.WorldIndexCoords.Y, actionCode);
            }
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

        private void UpdateModel()
        {
             ModelChangeAction?.Invoke(CurrentActionSetRef);
        }

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

        #endregion Private Methods
    }
}