﻿using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Maps;
using OpenBreed.Model.Maps.Blocks;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Texts
{
    public class TextFromMapEditorVM : TextEditorBaseVM<IDbTextFromMap>
    {
        #region Private Fields

        private string _blockName;
        private bool _editEnabled;

        private string _text;

        private string _dataRef;
        private readonly TextsDataProvider textsDataProvider;
        private readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public TextFromMapEditorVM(
            ILogger logger,
            TextsDataProvider textsDataProvider,
            IModelsProvider dataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(logger, workspaceMan, dialogProvider)
        {
            this.textsDataProvider = textsDataProvider;
            this.dataProvider = dataProvider;

            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public string BlockName
        {
            get { return _blockName; }
            set { SetProperty(ref _blockName, value); }
        }

        public bool EditEnabled
        {
            get { return _editEnabled; }
            set { SetProperty(ref _editEnabled, value); }
        }

        public string DataRef
        {
            get { return _dataRef; }
            set { SetProperty(ref _dataRef, value); }
        }

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        public override string EditorName => "MAP Text Editor";

        #endregion Public Properties

        #region Public Methods

        protected override void UpdateEntry(IDbTextFromMap entry)
        {
            var mapModel = dataProvider.GetModel<IDbTextFromMap, MapModel>(entry);

            var textBlock = mapModel.Blocks.OfType<MapTextBlock>().FirstOrDefault(item => item.Name == BlockName);

            textBlock.Value = Text;

            var model = textsDataProvider.GetText(entry);
            model.Text = Text;

            entry.MapRef = DataRef;
            entry.BlockName = BlockName;
        }

        protected override void UpdateVM(IDbTextFromMap entry)
        {
            var model = textsDataProvider.GetText(entry);

            if (model != null)
                Text = model.Text;

            DataRef = entry.MapRef;
            BlockName = entry.BlockName;
        }

        #endregion Public Methods

        #region Private Methods

        private void This_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(BlockName):
                case nameof(DataRef):
                    EditEnabled = ValidateSettings();
                    break;

                default:
                    break;
            }
        }

        private bool ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(DataRef))
                return false;

            if (string.IsNullOrWhiteSpace(BlockName))
                return false;

            return true;
        }

        #endregion Private Methods
    }
}