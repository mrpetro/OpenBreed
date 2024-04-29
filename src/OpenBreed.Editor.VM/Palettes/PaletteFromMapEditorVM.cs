using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Model.Maps.Blocks;
using OpenBreed.Model.Maps;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Collections.ObjectModel;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Common.Interface.Dialog;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteFromMapEditorVM : PaletteEditorBaseVM<IDbPaletteFromMap>
    {
        #region Private Fields

        private string _blockName;
        private bool editEnabled;
        private string dataRef;

        #endregion Private Fields

        #region Public Constructors

        public PaletteFromMapEditorVM(
            PalettesDataProvider palettesDataProvider,
            IModelsProvider dataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            IControlFactory controlFactory) : base(palettesDataProvider, dataProvider, workspaceMan, dialogProvider, controlFactory)
        {
            BlockNames = new BindingList<string>();
            BlockNames.ListChanged += (s, a) => OnPropertyChanged(nameof(BlockNames));

        }

        #endregion Public Constructors

        #region Public Properties

        public override string EditorName => "MAP Palette Editor";

        public string DataRef
        {
            get { return dataRef; }
            set { SetProperty(ref dataRef, value); }
        }

        public BindingList<string> BlockNames { get; }

        public string BlockName
        {
            get { return _blockName; }
            set { SetProperty(ref _blockName, value); }
        }

        public bool EditEnabled
        {
            get { return editEnabled; }
            set { SetProperty(ref editEnabled, value); }
        }

        #endregion Public Properties

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(BlockName):
                case nameof(DataRef):
                    EditEnabled = ValidateSettings();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void UpdatePaletteBlocksList(IDbPaletteFromMap source)
        {
            BlockNames.UpdateAfter(() =>
            {
                BlockNames.Clear();

                var map = dataProvider.GetModel<MapModel>(source.DataRef);

                if (map == null)
                    return;

                foreach (var paletteBlock in map.Blocks.OfType<MapPaletteBlock>())
                    BlockNames.Add(paletteBlock.Name);
            });
        }

        protected override void UpdateVM(IDbPaletteFromMap entry)
        {
            UpdatePaletteBlocksList(entry);

            var model = palettesDataProvider.GetPalette(entry.Id);

            if (model != null)
                UpdateVMColors(model);

            DataRef = entry.DataRef;
            BlockName = entry.BlockName;
        }

        protected override void UpdateEntry(IDbPaletteFromMap source)
        {
            var mapModel = dataProvider.GetModel<MapModel>(DataRef);

            var paletteBlock = mapModel.Blocks.OfType<MapPaletteBlock>().FirstOrDefault(item => item.Name == BlockName);

            for (int i = 0; i < paletteBlock.Value.Length; i++)
            {
                var color = Colors[i].Color;
                paletteBlock.Value[i] = new MapPaletteBlock.ColorData(color.R, color.G, color.B);
            }

            source.DataRef = DataRef;
            source.BlockName = BlockName;
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