using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
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

        private readonly IPictureMan pictureMan;
        private readonly ImagesDataProvider imagesDataProvider;
        private readonly ITextureMan textureMan;

        #endregion Private Fields

        #region Public Constructors

        public PictureDataLoader(
            ImagesDataProvider imagesDataProvider,
            ITextureMan textureMan,
            IPictureMan pictureMan)
        {
            this.imagesDataProvider = imagesDataProvider;
            this.textureMan = textureMan;
            this.pictureMan = pictureMan;
        }

        public IPicture Load(string entryId)
        {
            var picture = pictureMan.GetByName(entryId);

            if (picture is not null)
            {
                return picture;
            }

            var bitmap = imagesDataProvider.GetImageById(entryId);

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