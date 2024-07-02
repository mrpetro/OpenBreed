using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Model.Sounds;
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

        public IImage GetImage(IDbImage dbImage, bool refresh = false)
        {
            switch (dbImage)
            {
                case IDbIffImage dbIffImage:
                    return dataProvider.GetModel<IDbIffImage, IImage>(dbIffImage, refresh);
                case IDbAcbmImage dbAcbmImage:
                    return dataProvider.GetModel<IDbAcbmImage, IImage>(dbAcbmImage, refresh);
                default:
                    throw new NotImplementedException(dbImage.GetType().ToString());
            }
        }

        //public Image GetImage(string id)
        //{
        //    var entry = repositoryProvider.GetRepository<IDbImage>().GetById(id);
        //    if (entry == null)
        //        throw new Exception("Image error: " + id);

        //    if (entry.DataRef == null)
        //        return null;

        //    return dataProvider.GetModel<Image>(entry.DataRef);
        //}

        #endregion Public Methods
    }
}