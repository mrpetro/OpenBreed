using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Helpers;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Sprites;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetEditorVM : EntryEditorBaseVM<ISpriteSetEntry, SpriteSetVM>
    {
        #region Private Fields

        private string _currentPaletteId;

        private PaletteModel palette;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetEditorVM(IRepository repository) : base(repository)
        {
            PaletteIds = new BindingList<string>();
            PropertyChanged += This_PropertyChanged;
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

        public override string EditorName { get { return "Sprite Set Editor"; } }
        public BindingList<string> PaletteIds { get; }
        public int SelectedIndex { get; private set; }

        public IEntryEditor<ISpriteSetEntry, SpriteSetVM> Subeditor { get; private set; }

        #endregion Public Properties

        #region Internal Methods

        internal void SetupPaletteIds(List<string> paletteRefs, SpriteSetVM target)
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

        protected override SpriteSetVM CreateVM(ISpriteSetEntry entry)
        {
            if (entry is ISpriteSetFromSprEntry)
                Subeditor = new SpriteSetFromSprEditorVM(this);
            else if (entry is ISpriteSetFromImageEntry)
                Subeditor = new SpriteSetFromImageEditorVM(this);
            else
                throw new NotImplementedException();

            return Subeditor.CreateVM(entry);
        }

        protected override void UpdateEntry(SpriteSetVM vm, ISpriteSetEntry entry)
        {
            base.UpdateEntry(vm, entry);

            Subeditor.UpdateEntry(vm, entry);
        }

        protected override void UpdateVM(ISpriteSetEntry entry, SpriteSetVM vm)
        {
            base.UpdateVM(entry, vm);
            Subeditor.UpdateVM(entry, vm);
            SetupPaletteIds(entry.PaletteRefs, vm);
        }

        #endregion Protected Methods

        #region Private Methods

        private void SwitchPalette(string paletteId)
        {
            Palette = ServiceLocator.Instance.GetService<DataProvider>().Palettes.GetPalette(paletteId);
        }

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentPaletteId):
                    SwitchPalette(CurrentPaletteId);
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}