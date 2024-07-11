using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Model.Texts;
using OpenBreed.Model.Tiles;
using System;

namespace OpenBreed.Common.Data
{
    public class TextsDataProvider
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;

        private readonly IModelsProvider modelsProvider;

        #endregion Private Fields

        #region Public Constructors

        public TextsDataProvider(IModelsProvider dataProvider, IRepositoryProvider repositoryProvider)
        {
            this.modelsProvider = dataProvider;
            this.repositoryProvider = repositoryProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public TextModel GetText(IDbText dbText, bool refresh = false)
        {
            switch (dbText)
            {
                case IDbTextFromMap dbTextFromMap:
                    return modelsProvider.GetModel<IDbTextFromMap, TextModel>(dbTextFromMap, refresh);
                case IDbTextEmbedded dbTextEmbedded:
                    return modelsProvider.GetModel<IDbTextEmbedded, TextModel>(dbTextEmbedded, refresh);
                case IDbTextFromFile dbTextFromFile:
                    return modelsProvider.GetModel<IDbTextFromFile, TextModel>(dbTextFromFile, refresh);
                default:
                    throw new NotImplementedException(dbText.GetType().ToString());
            }
        }

        public TextModel GetTextById(string entryId)
        {
            var entry = repositoryProvider.GetRepository<IDbText>().GetById(entryId);

            if (entry is null)
            {
                return null;
            }

            return GetText(entry);
        }

        #endregion Public Methods

        #region Private Methods


        #endregion Private Methods
    }
}