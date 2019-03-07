using OpenBreed.Common.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Data
{
    public class SpriteSetsDataProvider
    {

        #region Public Constructors

        public SpriteSetsDataProvider(DataProvider provider)
        {
            Provider = provider;
        }

        #endregion Public Constructors

        #region Public Properties

        public DataProvider Provider { get; }

        #endregion Public Properties

        #region Public Methods

        public SpriteSetModel GetSpriteSet(string id)
        {
            var entry = Provider.UnitOfWork.GetRepository<ISpriteSetEntry>().GetById(id);
            if (entry == null)
                throw new Exception("SpriteSet error: " + id);

            if (entry.DataRef == null)
                return null;

            return Provider.Datas.GetData(entry.DataRef) as SpriteSetModel;
        }

        #endregion Public Methods

    }
}
