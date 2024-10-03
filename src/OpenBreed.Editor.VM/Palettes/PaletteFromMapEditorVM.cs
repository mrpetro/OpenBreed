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
using Microsoft.Extensions.Logging;
using OpenBreed.Database.Interface.Items.Maps;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteFromMapEditorVM : PaletteEditorBaseVM<IDbPaletteFromMap>
    {
        #region Private Fields

        private bool editEnabled;

        #endregion Private Fields

        #region Public Constructors

        public PaletteFromMapEditorVM(
            IDbPaletteFromMap dbEntry,
            ILogger logger,
            PalettesDataProvider palettesDataProvider,
            IModelsProvider dataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(dbEntry, logger, palettesDataProvider, dataProvider, workspaceMan, dialogProvider)
        {
            BlockNames = new BindingList<string>();
            BlockNames.ListChanged += (s, a) => OnPropertyChanged(nameof(BlockNames));

            IgnoreProperty(nameof(EditEnabled));
        }

        #endregion Public Constructors

        #region Public Properties

        public override string EditorName => "MAP Palette Editor";

        public string MapRef
        {
            get { return Entry.MapRef; }
            set { SetProperty(Entry, x => x.MapRef, value); }
        }

        public BindingList<string> BlockNames { get; }

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

        #endregion Public Properties

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(BlockName):
                case nameof(MapRef):
                    EditEnabled = ValidateSettings();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        protected override void ProtectedUpdateVM()
        {
            UpdatePaletteBlocksList(Entry);

            var model = palettesDataProvider.GetPalette(Entry);

            if (model != null)
            {
                UpdateVMColors(model);
            }
        }

        protected override void ProtectedUpdateEntry()
        {
            var mapModel = modelsProvider.GetModelById<IDbMap, MapModel>(MapRef);

            var paletteBlock = mapModel.Blocks.OfType<MapPaletteBlock>().FirstOrDefault(item => item.Name == BlockName);

            for (int i = 0; i < paletteBlock.Value.Length; i++)
            {
                var color = Colors[i].Color;
                paletteBlock.Value[i] = new MapPaletteBlock.ColorData(color.R, color.G, color.B);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void UpdatePaletteBlocksList(IDbPaletteFromMap source)
        {
            BlockNames.UpdateAfter(() =>
            {
                BlockNames.Clear();

                var map = modelsProvider.GetModelById<IDbMap, MapModel>(source.MapRef);

                if (map is null)
                {
                    return;
                }

                foreach (var paletteBlock in map.Blocks.OfType<MapPaletteBlock>())
                {
                    BlockNames.Add(paletteBlock.Name);
                }
            });
        }

        private bool ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(MapRef))
                return false;

            if (string.IsNullOrWhiteSpace(BlockName))
                return false;

            return true;
        }

        #endregion Private Methods
    }
}