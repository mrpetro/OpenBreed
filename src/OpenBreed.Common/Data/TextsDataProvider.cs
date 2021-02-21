using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Model.Texts;
using System;

namespace OpenBreed.Common.Data
{
    public class TextsDataProvider
    {
        #region Private Fields

        private readonly IWorkspaceMan workspaceMan;

        private readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public TextsDataProvider(IModelsProvider dataProvider, IWorkspaceMan workspaceMan)
        {
            this.dataProvider = dataProvider;
            this.workspaceMan = workspaceMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public TextModel GetText(string id)
        {
            var entry = workspaceMan.UnitOfWork.GetRepository<ITextEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Text error: " + id);

            return GetModel(entry);
        }

        #endregion Public Methods

        #region Private Methods

        private TextModel GetModelImpl(ITextFromMapEntry entry)
        {
            return TextsDataHelper.FromMapModel(dataProvider, entry);
        }

        private TextModel GetModelImpl(ITextEmbeddedEntry entry)
        {
            return TextsDataHelper.FromBinary(dataProvider, entry);
        }

        private TextModel GetModel(dynamic entry)
        {
            return GetModelImpl(entry);
        }

        #endregion Private Methods
    }
}