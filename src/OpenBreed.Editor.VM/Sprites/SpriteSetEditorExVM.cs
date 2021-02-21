using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Palettes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetEditorExVM : BaseViewModel, IEntryEditor<ISpriteSetEntry>
    {
        #region Private Fields

        private string _currentPaletteId;

        private PaletteModel palette;
        protected readonly SpriteSetsDataProvider spriteSetsDataProvider;
        protected readonly PalettesDataProvider palettesDataProvider;
        protected readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetEditorExVM(SpriteSetsDataProvider spriteSetsDataProvider,
                                   PalettesDataProvider palettesDataProvider,
                                   IModelsProvider dataProvider)
        {
            this.spriteSetsDataProvider = spriteSetsDataProvider;
            this.palettesDataProvider = palettesDataProvider;
            this.dataProvider = dataProvider;
            PaletteIds = new BindingList<string>();
        }

        #endregion Public Constructors

        #region Public Properties

        public ParentEntryEditor<ISpriteSetEntry> Parent { get; }

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

        #region Public Methods

        public virtual void UpdateEntry(ISpriteSetEntry entry)
        {
        }

        public virtual void UpdateVM(ISpriteSetEntry entry)
        {
            SetupPaletteIds(entry.PaletteRefs);
            SwitchPalette(CurrentPaletteId);
        }

        #endregion Public Methods

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

        #region Private Methods

        private void SwitchPalette(string paletteId)
        {
            Palette = palettesDataProvider.GetPalette(paletteId);
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

        #endregion Private Methods
    }
}