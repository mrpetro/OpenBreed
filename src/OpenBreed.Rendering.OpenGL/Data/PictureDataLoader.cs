using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Managers;
using System;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Rendering.OpenGL.Data
{
    public class PictureDataLoader : IPictureDataLoader
    {
        #region Private Fields

        private readonly AssetsDataProvider assetsDataProvider;
        private readonly IPictureMan pictureMan;
        private readonly IRepositoryProvider repositoryProvider;
        private readonly ITextureMan textureMan;

        #endregion Private Fields

        #region Public Constructors

        public PictureDataLoader(IRepositoryProvider repositoryProvider,
                                 AssetsDataProvider assetsDataProvider,
                                 ITextureMan textureMan,
                                 IPictureMan pictureMan)
        {
            this.repositoryProvider = repositoryProvider;
            this.assetsDataProvider = assetsDataProvider;
            this.textureMan = textureMan;
            this.pictureMan = pictureMan;
        }

        public object LoadObject(string entryId) => Load(entryId);

        public IPicture Load(string entryId, params object[] args)
        {
            var picture = pictureMan.GetByName(entryId);

            if (picture is not null)
                return picture;

            var entry = repositoryProvider.GetRepository<IDbImage>().GetById(entryId);
            if (entry is null)
                throw new Exception("Image error: " + entryId);

            var bitmap = assetsDataProvider.LoadModel(entry.DataRef) as IImage;

            var bytes = bitmap.GetBytes();

            var texture = textureMan.Create(entryId, bitmap.Width, bitmap.Height, bytes);

            return pictureMan.CreatePicture()
                .SetName(entryId)
                .SetTexture(texture.Id)
                .Build();
        }

        #endregion Public Constructors
    }
}