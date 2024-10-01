using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Model.Maps;
using OpenBreed.Model.Maps.Blocks;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Texts
{
    public class TextFromMapEditorVM : TextEditorBaseVM<IDbTextFromMap>
    {
        #region Private Fields

        private readonly TextsDataProvider textsDataProvider;
        private readonly IModelsProvider dataProvider;
        private bool editEnabled;
        private string text;

        #endregion Private Fields

        #region Public Constructors

        public TextFromMapEditorVM(
            IDbTextFromMap dbEntry,
            ILogger logger,
            TextsDataProvider textsDataProvider,
            IModelsProvider dataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
            this.textsDataProvider = textsDataProvider;
            this.dataProvider = dataProvider;

            MapRefIdEditor = new EntryRefIdEditorVM(
                workspaceMan,
                typeof(IDbMap),
                (newRefId) => DataRef = newRefId);

            MapRefIdEditor.SelectedRefId = Entry.MapRef;

            UpdateVM();
        }

        #endregion Public Constructors

        #region Public Properties

        public EntryRefIdEditorVM MapRefIdEditor { get; }

        public string BlockName
        {
            get { return Entry.BlockName; }
            set { SetProperty(Entry, x => x.BlockName, value); }
        }

        public bool EditEnabled
        {
            get { return editEnabled; }
            set { SetProperty(ref editEnabled, value); }
        }

        public string DataRef
        {
            get { return Entry.MapRef; }
            set { SetProperty(Entry, x => x.MapRef, value); }
        }

        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }

        public override string EditorName => "MAP Text Editor";

        #endregion Public Properties

        #region Protected Methods

        protected override void UpdateEntry()
        {
            var mapModel = dataProvider.GetModel<IDbTextFromMap, MapModel>(Entry);

            var textBlock = mapModel.Blocks.OfType<MapTextBlock>().FirstOrDefault(item => item.Name == BlockName);

            textBlock.Value = Text;

            var model = textsDataProvider.GetText(Entry);
            model.Text = Text;
        }

        protected override void UpdateVM()
        {
            var model = textsDataProvider.GetText(Entry);

            if (model != null)
            {
                Text = model.Text;
            }

            EditEnabled = ValidateSettings();
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(BlockName):
                case nameof(DataRef):
                    MapRefIdEditor.CurrentRefId = (DataRef == null) ? null : DataRef;
                    UpdateVM();
                    break;
                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private bool ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(DataRef))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(BlockName))
            {
                return false;
            }

            return true;
        }

        #endregion Private Methods
    }
}