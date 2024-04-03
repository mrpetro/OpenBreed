using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
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
    public abstract class SpriteSetEditorBaseVM<TSpriteAtlas> : EntryEditorBaseVM<IDbSpriteAtlas>
    {
        #region Protected Fields

        protected readonly SpriteAtlasDataProvider spriteAtlasDataProvider;
        protected readonly PalettesDataProvider palettesDataProvider;
        protected readonly IModelsProvider dataProvider;

        #endregion Protected Fields

        #region Private Fields

        private string _currentPaletteId;

        private PaletteModel palette;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetEditorBaseVM(
            SpriteAtlasDataProvider spriteAtlasDataProvider,
            PalettesDataProvider palettesDataProvider,
            IModelsProvider dataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            IControlFactory controlFactory) : base(workspaceMan, dialogProvider, controlFactory)
        {
            this.spriteAtlasDataProvider = spriteAtlasDataProvider;
            this.palettesDataProvider = palettesDataProvider;
            this.dataProvider = dataProvider;
            PaletteIds = new BindingList<string>();
        }

        #endregion Public Constructors

        #region Public Properties

        public string CurrentPaletteId
        {
            get { return _currentPaletteId; }
            set { SetProperty(ref _currentPaletteId, value); }
        }

        public PaletteModel Palette
        {
            get { return palette; }
            set { SetProperty(ref palette, value); }
        }

        public BindingList<string> PaletteIds { get; }
        public int SelectedIndex { get; private set; }

        #endregion Public Properties

        #region Internal Methods

        internal void SetupPaletteIds(List<string> paletteRefs)
        {
            PaletteIds.UpdateAfter(() =>
            {
                PaletteIds.Clear();
                paletteRefs.ForEach(item => PaletteIds.Add(item));
            });

            CurrentPaletteId = PaletteIds.FirstOrDefault();
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
            SetupPaletteIds(entry.PaletteRefs);
            SwitchPalette(CurrentPaletteId);

            UpdateVM((TSpriteAtlas)entry);
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(CurrentPaletteId):
                    SwitchPalette(CurrentPaletteId);
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