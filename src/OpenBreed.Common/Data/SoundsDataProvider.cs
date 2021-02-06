using OpenBreed.Model.Sounds;
using OpenBreed.Database.Interface.Items.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Database.Interface;

namespace OpenBreed.Common.Data
{
    public class SoundsDataProvider
    {
        private readonly IUnitOfWork unitOfWork;
        #region Public Constructors

        public SoundsDataProvider(DataProvider provider, IUnitOfWork unitOfWork)
        {
            Provider = provider;
            this.unitOfWork = unitOfWork;
        }

        #endregion Public Constructors

        #region Public Properties

        public DataProvider Provider { get; }

        #endregion Public Properties

        public SoundModel GetSound(string id)
        {
            var entry = unitOfWork.GetRepository<ISoundEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Sound error: " + id);

            if (entry.DataRef == null)
                return null;

            return Provider.GetData<SoundModel>(entry.DataRef);
        }

    }
}
