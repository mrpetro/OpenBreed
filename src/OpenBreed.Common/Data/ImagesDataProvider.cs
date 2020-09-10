using OpenBreed.Database.Interface.Items.Images;
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

        public Image GetImage(string id)
        {
            var entry = Provider.UnitOfWork.GetRepository<IImageEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Image error: " + id);

            if (entry.DataRef == null)
                return null;

            return Provider.GetData<Image>(entry.DataRef);
        }

    }
}
