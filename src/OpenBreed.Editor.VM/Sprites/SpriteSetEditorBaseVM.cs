using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Palettes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Sprites
{
    public abstract class SpriteSetEditorBaseVM<TSpriteAtlas> : EntrySpecificEditorVM<IDbSpriteAtlas> where TSpriteAtlas : IDbSpriteAtlas
    {
        #region Protected Fields

        protected readonly SpriteAtlasDataProvider spriteAtlasDataProvider;
        protected readonly PalettesDataProvider palettesDataProvider;
        protected readonly IModelsProvider dataProvider;

        #endregion Protected Fields

        #region Private Fields

        private string _currentPaletteRef;

        private PaletteModel palette;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetEditorBaseVM(
            TSpriteAtlas dbEntry,
            ILogger logger,
            SpriteAtlasDataProvider spriteAtlasDataProvider,
            PalettesDataProvider palettesDataProvider,
            IModelsProvider modelsProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
            this.spriteAtlasDataProvider = spriteAtlasDataProvider;
            this.palettesDataProvider = palettesDataProvider;
            this.dataProvider = modelsProvider;
            PaletteRefs = new BindingList<string>();

            palette = PaletteModel.NullPalette;

        }

        #endregion Public Constructors

        #region Public Properties

        public string CurrentPaletteRef
        {
            get { return _currentPaletteRef; }
            set { SetProperty(ref _currentPaletteRef, value); }
        }

        public PaletteModel Palette
        {
            get { return palette; }
            set { SetProperty(ref palette, value); }
        }

        public BindingList<string> PaletteRefs { get; }
        public int SelectedIndex { get; private set; }

        #endregion Public Properties

        #region Internal Methods

        internal void SetupPaletteRefs(List<string> paletteRefs)
        {
            PaletteRefs.UpdateAfter(() =>
            {
                PaletteRefs.Clear();
                paletteRefs.ForEach(item => PaletteRefs.Add(item));
            });

            CurrentPaletteRef = PaletteRefs.FirstOrDefault();
        }

        #endregion Internal Methods

        #region Protected Methods

        protected abstract void UpdateEntry(TSpriteAtlas target);

        protected abstract void UpdateVM(TSpriteAtlas source);

        protected override void UpdateEntry(IDbSpriteAtlas entry)
        {
            UpdateEntry((TSpriteAtlas)entry);
        }

        protected override void UpdateVM(IDbSpriteAtlas entry)
        {
            UpdateVM((TSpriteAtlas)entry);
            SetupPaletteRefs(entry.PaletteRefs);
            SwitchPalette(CurrentPaletteRef);

        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(CurrentPaletteRef):
                    SwitchPalette(CurrentPaletteRef);
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void SwitchPalette(string paletteId)
        {
            Palette = palettesDataProvider.GetPalette(paletteId);
        }

        #endregion Private Methods
    }
}