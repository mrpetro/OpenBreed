using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteFromBinaryVM : PaletteVM
    {
        #region Public Constructors

        #endregion Public Constructors

        #region Internal Methods

        internal override void FromEntry(IPaletteEntry entry)
        {
            base.FromEntry(entry);
            FromEntry((IPaletteFromBinaryEntry)entry);
        }

        internal override void ToEntry(IPaletteEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((IPaletteFromBinaryEntry)entry);
        }

        #endregion Internal Methods

        #region Private Methods

        private void FromEntry(IPaletteFromBinaryEntry entry)
        {
            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var model = dataProvider.Palettes.GetPalette(entry.Id);

            if (model != null)
            {
                Colors.UpdateAfter(() =>
                {
                    for (int i = 0; i < model.Data.Length; i++)
                        Colors[i] = model.Data[i];
                });

                CurrentColorIndex = 0;
            }

            DataRef = entry.DataRef;
        }

        private void ToEntry(IPaletteFromBinaryEntry entry)
        {
            entry.DataRef = DataRef;
        }

        #endregion Private Methods
    }
}