using OpenBreed.Common;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorPalettesToolVM : MapEditorToolVM
    {
        #region Private Fields

        private string currentPaletteRef;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorPalettesToolVM(MapEditorVM parent)
        {
            Parent = parent;

            PaletteNames = new BindingList<string>();
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<string> PaletteNames { get; }

        public Action<string> ModelChangeAction { get; internal set; }

        public string CurrentPaletteRef
        {
            get { return currentPaletteRef; }
            set { SetProperty(ref currentPaletteRef, value); }
        }

        public MapEditorVM Parent { get; }

        #endregion Public Properties

        #region Internal Methods

        internal void UpdateList(IEnumerable<string> paletteRefs)
        {
            PaletteNames.UpdateAfter(() =>
            {
                PaletteNames.Clear();
                paletteRefs.ForEach(item => PaletteNames.Add(item));
            });

            CurrentPaletteRef = PaletteNames.FirstOrDefault();
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(CurrentPaletteRef):
                    UpdateModel();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void UpdateModel()
        {
            ModelChangeAction?.Invoke(CurrentPaletteRef);
        }

        #endregion Private Methods
    }
}