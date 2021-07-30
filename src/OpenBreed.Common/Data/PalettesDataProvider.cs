using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Model.Palettes;
using System;

namespace OpenBreed.Common.Data
{
    public class PalettesDataProvider
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;

        private readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public PalettesDataProvider(IModelsProvider dataProvider, IRepositoryProvider repositoryProvider)
        {
            this.dataProvider = dataProvider;
            this.repositoryProvider = repositoryProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public PaletteModel GetPalette(string id)
        {
            var entry = repositoryProvider.GetRepository<IDbPalette>().GetById(id);
            if (entry == null)
                throw new Exception("Palette error: " + id);

            return GetModel(entry);
        }

        #endregion Public Methods

        #region Private Methods

        private PaletteModel GetModelImpl(IDbPaletteFromMap entry)
        {
            return PalettesDataHelper.FromMapModel(dataProvider, entry);
        }

        private PaletteModel GetModelImpl(IDbPaletteFromBinary entry)
        {
            return PalettesDataHelper.FromBinary(dataProvider, entry);
        }

        private PaletteModel GetModel(dynamic entry)
        {
            return GetModelImpl(entry);
        }

        #endregion Private Methods
    }
}