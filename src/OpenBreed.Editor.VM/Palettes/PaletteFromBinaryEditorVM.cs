using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteFromBinaryEditorVM : PaletteEditorExVM
    {
        #region Public Constructors

        private string _dataRef;

        public PaletteFromBinaryEditorVM(PaletteEditorVM parent) : base(parent)
        {
        }
        public string DataRef
        {
            get { return _dataRef; }
            set { SetProperty(ref _dataRef, value); }
        }

        #endregion Public Constructors

        #region Public Properties

        #endregion Public Properties

        #region Public Methods

        public override void UpdateVM(IPaletteEntry entry)
        {
            base.UpdateVM(entry);
            UpdateVM((IPaletteFromBinaryEntry)entry);
        }

        public override void UpdateEntry(IPaletteEntry entry)
        {
            base.UpdateEntry(entry);
            UpdateEntry((IPaletteFromBinaryEntry)entry);
        }

        #endregion Public Methods

        #region Private Methods

        private void UpdateVM(IPaletteFromBinaryEntry entry)
        {
            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var model = dataProvider.Palettes.GetPalette(entry.Id);

            if (model != null)
                UpdateVMColors(model);

            DataRef = entry.DataRef;
        }

        private void UpdateEntry(IPaletteFromBinaryEntry entry)
        {
            entry.DataRef = DataRef;
        }

        #endregion Private Methods
    }
}