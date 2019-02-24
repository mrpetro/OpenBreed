using OpenBreed.Common.Images;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Data
{
    public class ImagesDataProvider
    {
        #region Public Constructors

        public ImagesDataProvider(DataProvider provider)
        {
            Provider = provider;
        }

        #endregion Public Constructors

        #region Public Properties

        public DataProvider Provider { get; }

        #endregion Public Properties

        public Image GetImage(string name)
        {
            var entry = Provider.UnitOfWork.GetRepository<IImageEntry>().GetById(name);
            if (entry == null)
                throw new Exception("Image error: " + name);

            if (entry.AssetRef == null)
                return null;

            var asset = Provider.Assets.GetAsset(entry.AssetRef);

            var image = Provider.FormatMan.Load(asset, entry.Format) as Image;
            image.Tag = name;
            return image;
        }

    }
}
