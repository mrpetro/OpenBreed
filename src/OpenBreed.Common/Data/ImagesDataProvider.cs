using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Images;
using System;
using System.Drawing;

namespace OpenBreed.Common.Data
{
    public class ImagesDataProvider
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;

        private readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public ImagesDataProvider(IModelsProvider dataProvider, IRepositoryProvider repositoryProvider)
        {
            this.dataProvider = dataProvider;
            this.repositoryProvider = repositoryProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public Image GetImage(string id)
        {
            var entry = repositoryProvider.GetRepository<IImageEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Image error: " + id);

            if (entry.DataRef == null)
                return null;

            return dataProvider.GetModel<Image>(entry.DataRef);
        }

        #endregion Public Methods
    }
}