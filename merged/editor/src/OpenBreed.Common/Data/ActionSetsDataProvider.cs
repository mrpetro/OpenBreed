using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Model.Actions;
using System;

namespace OpenBreed.Common.Data
{
    public class ActionSetsDataProvider
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly IDrawingFactory drawingFactory;
        private readonly IDrawingContextProvider drawingContextProvider;
        private readonly IImageProvider imageProvider;

        #endregion Private Fields

        #region Public Constructors

        public ActionSetsDataProvider(
            IModelsProvider modelsProvider,
            IRepositoryProvider repositoryProvider,
            IDrawingFactory drawingFactory,
            IDrawingContextProvider drawingContextProvider,
            IImageProvider imageProvider)
        {
            Provider = modelsProvider;
            this.repositoryProvider = repositoryProvider;
            this.drawingFactory = drawingFactory;
            this.drawingContextProvider = drawingContextProvider;
            this.imageProvider = imageProvider;
        }

        #endregion Public Constructors

        #region Public Properties

        public IModelsProvider Provider { get; }

        #endregion Public Properties

        #region Public Methods

        public ActionSetModel GetActionSet(string id)
        {
            var entry = repositoryProvider.GetRepository<IDbActionSet>().GetById(id);
            if (entry == null)
                throw new Exception("ActionSet error: " + id);

            return GetModel(entry);
        }

        #endregion Public Methods

        #region Private Methods

        private ActionSetModel GetModelImpl(IDbActionSet entry)
        {
            return ActionSetsDataHelper.FromEmbeddedData(drawingFactory, drawingContextProvider, imageProvider, Provider, entry);
        }

        private ActionSetModel GetModel(dynamic entry)
        {
            return GetModelImpl(entry);
        }

        #endregion Private Methods
    }
}