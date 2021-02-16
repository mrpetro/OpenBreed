using OpenBreed.Database.Interface.Items.Images;
using System;
using System.Drawing;

namespace OpenBreed.Common.Data
{
    public class ImagesDataProvider
    {
        #region Private Fields

        private readonly IWorkspaceMan workspaceMan;

        private readonly IDataProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public ImagesDataProvider(IDataProvider dataProvider, IWorkspaceMan workspaceMan)
        {
            this.dataProvider = dataProvider;
            this.workspaceMan = workspaceMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public Image GetImage(string id)
        {
            var entry = workspaceMan.UnitOfWork.GetRepository<IImageEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Image error: " + id);

            if (entry.DataRef == null)
                return null;

            return dataProvider.GetData<Image>(entry.DataRef);
        }

        #endregion Public Methods
    }
}