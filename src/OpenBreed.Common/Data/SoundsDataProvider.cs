using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Model.Sounds;
using System;

namespace OpenBreed.Common.Data
{
    public class SoundsDataProvider
    {
        #region Private Fields

        private readonly IWorkspaceMan workspaceMan;

        private readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public SoundsDataProvider(IModelsProvider dataProvider, IWorkspaceMan workspaceMan)
        {
            this.dataProvider = dataProvider;
            this.workspaceMan = workspaceMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public SoundModel GetSound(string id)
        {
            var entry = workspaceMan.UnitOfWork.GetRepository<ISoundEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Sound error: " + id);

            if (entry.DataRef == null)
                return null;

            return dataProvider.GetModel<SoundModel>(entry.DataRef);
        }

        #endregion Public Methods
    }
}