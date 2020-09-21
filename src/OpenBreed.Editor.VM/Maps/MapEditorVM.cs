using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Actions;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorVM : EntryEditorBaseVM<IMapEntry, MapVM>
    {
        #region Private Fields

        private string actionSetRef;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorVM(IRepository repository) : base(repository)
        {
            Tools = new MapEditorToolsVM();

            TilesTool = new MapEditorTilesToolVM(this);
            ActionsTool = new MapEditorActionsToolVM(this);
            PalettesTool = new MapEditorPalettesToolVM(this);

            MapView = new MapEditorViewVM(this);

            PalettesTool.PropertyChanged += PaletteSelector_PropertyChanged;

            ActionsTool.PropertyChanged += ActionsTool_PropertyChanged;

            InitializeTools();

            //TODO: This is probably bad place for VM connection method
            Connect();
        }

        #endregion Public Constructors

        #region Public Properties

        public ActionSetModel ActionSet { get; private set; }

        public MapEditorActionsToolVM ActionsTool { get; }

        public override string EditorName { get { return "Level Editor"; } }

        public MapEditorViewVM MapView { get; }

        public MapEditorPalettesToolVM PalettesTool { get; }

        public MapEditorTilesToolVM TilesTool { get; }

        public MapEditorToolsVM Tools { get; }

        public string ActionSetRef
        {
            get { return actionSetRef; }
            set { SetProperty(ref actionSetRef, value); }
        }

        #endregion Public Properties

        #region Internal Methods

        internal void Connect()
        {
            ActionsTool.ActionsSelector.PropertyChanged += ActionsSelector_PropertyChanged;

            TilesTool.Connect();
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void UpdateEntry(MapVM source, IMapEntry target)
        {
            base.UpdateEntry(source, target);

            var mapEntry = target as IMapEntry;

            mapEntry.ActionSetRef = ActionSetRef != null ? ActionSetRef : null;
        }

        protected override void UpdateVM(IMapEntry source, MapVM target)
        {
            base.UpdateVM(source, target);

            ActionSetRef = source.ActionSetRef;
            ActionsTool.ActionSetRef = source.ActionSetRef;
        }

        #endregion Protected Methods

        #region Private Methods

        private void UpdateActionModel()
        {
            if (ActionSetRef == null)
            {
                ActionSet = null;
                return;
            }

            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var actionSet = dataProvider.ActionSets.GetActionSet(ActionSetRef);
            if (actionSet != null)
                ActionSet = actionSet;
        }

        private void ActionsSelector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var actionSelector = sender as MapEditorActionsSelectorVM;

            switch (e.PropertyName)
            {
                case nameof(actionSelector.Items):
                    MapView.Refresh();
                    break;

                default:
                    break;
            }
        }

        private void ActionsTool_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ActionsTool.ActionSetRef):
                    if (Editable != null)
                        ActionSetRef = ActionsTool.ActionSetRef;
                    break;

                default:
                    break;
            }
        }

        private void InitializeTool(MapEditorToolVM tool)
        {
        }

        private void InitializeTools()
        {
            Tools.Items.UpdateAfter(() =>
            {
                Tools.Items.Add(TilesTool);
                Tools.Items.Add(ActionsTool);
                Tools.Items.Add(PalettesTool);
            });

            Tools.CurrentTool = Tools.Items.FirstOrDefault();

            MapView.Cursor.UpdateAction = Tools.OnCursorUpdate;
        }

        private void PaletteSelector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(PalettesTool.CurrentItem):
                    foreach (var tileSet in Editable.TileSets)
                        tileSet.Palette = PalettesTool.CurrentItem;
                    break;

                default:
                    break;
            }
        }

        //    base.UpdateVM(source, target);
        //}

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(Editable):
                    MapView.Layout = Editable.Layout;
                    PalettesTool.CurrentItem = Editable.Palettes.FirstOrDefault();
                    ActionSetRef = ActionsTool.ActionSetRef;
                    break;
                case nameof(ActionSetRef):
                    UpdateActionModel();
                    break;
                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Private Methods

        //    TileSelector.CurrentItem = CurrentLevel.TileSets.FirstOrDefault();
        //    PropSelector.CurrentItem = CurrentLevel.PropSet;
        //    SpriteSetViewer.CurrentItem = CurrentLevel.SpriteSets.FirstOrDefault();
        //    PaletteSelector.CurrentItem = CurrentLevel.Palettes.FirstOrDefault();
        //}
        //protected override void UpdateVM(IMapEntry source, MapVM target)
        //{
        //    target.FromEntry(source);
        //public LevelTileSelectorVM TileSelector { get; }
        //public void Load(string name)
        //{
        //    CurrentLevel = Root.CreateLevel();
        //    //CurrentLevel.Load(name);
        //private void PaletteSelector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    var paletteSelector = sender as LevelPaletteSelectorVM;

        //    switch (e.PropertyName)
        //    {
        //        case nameof(paletteSelector.CurrentItem):
        //            Root.PaletteEditor.Editable = paletteSelector.CurrentItem;
        //            Root.PaletteEditor.CurrentColorIndex = 0;

        //            foreach (var tileSet in TileSets)
        //                tileSet.Palette = paletteSelector.CurrentItem;

        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}