using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Model.Palettes;
using System;

namespace OpenBreed.Common.Data
{
    public class PalettesDataProvider
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;

        private readonly IModelsProvider modelsProvider;

        #endregion Private Fields

        #region Public Constructors

        public PalettesDataProvider(IModelsProvider modelsProvider, IRepositoryProvider repositoryProvider)
        {
            this.modelsProvider = modelsProvider;
            this.repositoryProvider = repositoryProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public PaletteModel GetPalette(string id)
        {
            var entry = repositoryProvider.GetRepository<IDbPalette>().GetById(id);
            if (entry is null)
            {
                throw new Exception("Palette error: " + id);
            }

            return GetPalette(entry);
        }

        public PaletteModel GetPalette(IDbPalette dbPalette, bool refresh = false)
        {
            switch (dbPalette)
            {
                case IDbPaletteFromLbm dbPaletteFromLbm:
                    return modelsProvider.GetModel<IDbPaletteFromLbm, PaletteModel>(dbPaletteFromLbm, refresh);
                case IDbPaletteFromBinary dbPaletteFromBinary:
                    return modelsProvider.GetModel<IDbPaletteFromBinary, PaletteModel>(dbPaletteFromBinary, refresh);
                case IDbPaletteFromMap dbPaletteFromMap:
                    return modelsProvider.GetModel<IDbPaletteFromMap, PaletteModel>(dbPaletteFromMap, refresh);
                default:
                    throw new NotImplementedException(dbPalette.GetType().ToString());
            }
        }

        #endregion Public Methods

    }
}