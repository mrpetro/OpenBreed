using OpenBreed.Model.Palettes;
using OpenBreed.Database.Interface.Items.Palettes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Database.Interface;

namespace OpenBreed.Common.Data
{
    public class PalettesDataProvider
    {
        private readonly IUnitOfWork unitOfWork;
        #region Public Constructors

        public PalettesDataProvider(DataProvider provider, IUnitOfWork unitOfWork)
        {
            Provider = provider;
            this.unitOfWork = unitOfWork;
        }

        #endregion Public Constructors

        #region Public Properties

        public DataProvider Provider { get; }

        #endregion Public Properties


        private PaletteModel GetModelImpl(IPaletteFromMapEntry entry)
        {
            return PalettesDataHelper.FromMapModel(Provider, entry);
        }

        private PaletteModel GetModelImpl(IPaletteFromBinaryEntry entry)
        {
            return PalettesDataHelper.FromBinary(Provider, entry);
        }

        private PaletteModel GetModel(dynamic entry)
        {
            return GetModelImpl(entry);
        }

        public PaletteModel GetPalette(string id)
        {
            var entry = unitOfWork.GetRepository<IPaletteEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Palette error: " + id);

            return GetModel(entry);
        }
    }
}
