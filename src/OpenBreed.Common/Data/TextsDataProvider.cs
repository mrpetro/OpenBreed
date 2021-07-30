using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Model.Texts;
using System;

namespace OpenBreed.Common.Data
{
    public class TextsDataProvider
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;

        private readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public TextsDataProvider(IModelsProvider dataProvider, IRepositoryProvider repositoryProvider)
        {
            this.dataProvider = dataProvider;
            this.repositoryProvider = repositoryProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public TextModel GetText(string id)
        {
            var entry = repositoryProvider.GetRepository<IDbText>().GetById(id);
            if (entry == null)
                throw new Exception("Text error: " + id);

            return GetModel(entry);
        }

        #endregion Public Methods

        #region Private Methods

        private TextModel GetModelImpl(IDbTextFromMap entry)
        {
            return TextsDataHelper.FromMapModel(dataProvider, entry);
        }

        private TextModel GetModelImpl(IDbTextEmbedded entry)
        {
            return TextsDataHelper.FromBinary(dataProvider, entry);
        }

        private TextModel GetModel(dynamic entry)
        {
            return GetModelImpl(entry);
        }

        #endregion Private Methods
    }
}