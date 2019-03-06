using OpenBreed.Common.Palettes;
using System;
using System.Collections.Generic;
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


        public PaletteModel GetPalette(string id)
        {
            var entry = Provider.UnitOfWork.GetRepository<IPaletteEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Palette error: " + id);

            if (entry.DataRef == null)
                return null;

            return Provider.Datas.GetData(entry.DataRef) as PaletteModel;
        }
    }
}
