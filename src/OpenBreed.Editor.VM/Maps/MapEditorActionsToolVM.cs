using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Editor.VM.Actions;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Editor.VM.Maps.Layers;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorActionsToolVM : MapEditorToolVM
    {
        #region Private Fields

        private string actionSetRef;

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

        public string ActionSetRef
        {
            get { return actionSetRef; }
            set { SetProperty(ref actionSetRef, value); }
        }

        public EntryRefIdEditorVM RefIdEditor { get; }

        public MapEditorActionsSelectorVM ActionsSelector { get; }

        public MapEditorVM Parent { get; }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(ActionSetRef):
                    RefIdEditor.RefId = ActionSetRef;
                    break;
                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Public Properties

        #region Internal Methods

        internal override void OnCursor(MapViewCursorVM cursor)
        {
            if ((cursor.Action == CursorActions.Move || cursor.Action == CursorActions.Down) && cursor.Buttons.HasFlag(CursorButtons.Left))
            {
                var actionLayer = Parent.Editable.Layout.Layers.OfType<MapLayerActionVM>().FirstOrDefault();

                var actionCode = ActionsSelector.SelectedIndex;

                actionLayer.SetCell(cursor.WorldIndexCoords.X, cursor.WorldIndexCoords.Y, actionCode);
            }
        }

        #endregion Internal Methods

        #region Private Methods

        private void ActionEntryRef_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(RefIdEditor.RefId):
                    ActionSetRef = RefIdEditor.RefId;
                    break;

                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}