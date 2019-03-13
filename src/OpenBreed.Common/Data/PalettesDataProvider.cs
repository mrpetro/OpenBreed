using OpenBreed.Common.Maps;
using OpenBreed.Common.Maps.Blocks;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Palettes.Builders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Data
{
    public class PalettesDataProvider
    {
        #region Public Constructors

        public PalettesDataProvider(DataProvider provider)
        {
            Provider = provider;
        }

        #endregion Public Constructors

        #region Public Properties

        public DataProvider Provider { get; }

        #endregion Public Properties


        private PaletteModel GetModelImpl(IPaletteFromMapEntry paletteData)
        {
            return PalettesDataHelper.FromMapModel(Provider, paletteData);
        }

        private PaletteModel GetModelImpl(IPaletteFromBinaryEntry paletteData)
        {
            return PalettesDataHelper.FromBinary(Provider, paletteData);
        }

        private PaletteModel GetModel(dynamic paletteEntry)
        {
            return GetModelImpl(paletteEntry);
        }

        public PaletteModel GetPalette(string id)
        {
            var entry = Provider.UnitOfWork.GetRepository<IPaletteEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Palette error: " + id);

            return GetModel(entry);
        }
    }
}
