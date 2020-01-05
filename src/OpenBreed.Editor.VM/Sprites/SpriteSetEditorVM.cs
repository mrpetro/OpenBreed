using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Sprites;
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

        public override string EditorName { get { return "Sprite Set Editor"; } }
        public BindingList<string> PaletteIds { get; }
        public int SelectedIndex { get; private set; }
        //public SpriteSetFromSprVM SpriteSetViewer { get; }

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

            SwitchPalette(CurrentPaletteId, target);
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override SpriteSetVM CreateVM(ISpriteSetEntry entry)
        {
            if (entry is ISpriteSetFromSprEntry)
                return new SpriteSetFromSprVM();
            else if (entry is ISpriteSetFromImageEntry)
                return new SpriteSetFromImageVM();
            else
                throw new NotImplementedException();
        }

        protected override void UpdateEntry(SpriteSetVM source, ISpriteSetEntry entry)
        {
            base.UpdateEntry(source, entry);
        }

        protected override void UpdateVM(ISpriteSetEntry entry, SpriteSetVM target)
        {
            base.UpdateVM(entry, target);
            SetupPaletteIds(entry.PaletteRefs, target);
        }

        #endregion Protected Methods

        #region Private Methods

        private void SwitchPalette(string paletteId, SpriteSetVM target)
        {
            if (target == null)
                return;

            if (paletteId == null)
                return;

            target.Palette = ServiceLocator.Instance.GetService<DataProvider>().Palettes.GetPalette(paletteId);
        }

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentPaletteId):
                    SwitchPalette(CurrentPaletteId, Editable);
                    break;

                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}