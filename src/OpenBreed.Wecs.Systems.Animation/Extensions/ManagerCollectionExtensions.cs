using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Animation.Commands;

namespace OpenBreed.Wecs.Systems.Animation.Extensions
{
    public static class ManagerCollectionExtensions
    {
        #region Public Methods

        public static void SetupAnimationSystems(this IManagerCollection manCollection)
        {
            var systemFactory = manCollection.GetManager<ISystemFactory>();
            systemFactory.Register(() => new AnimationSystem(manCollection.GetManager<IEntityMan>(),
                                                             manCollection.GetManager<IAnimationMan>(),
                                                             manCollection.GetManager<ILogger>()));

            var entityCommandHandler = manCollection.GetManager<EntityCommandHandler>();

            entityCommandHandler.BindCommand<SetAnimCommand, AnimationSystem>();
            entityCommandHandler.BindCommand<PlayAnimCommand, AnimationSystem>();
            entityCommandHandler.BindCommand<PauseAnimCommand, AnimationSystem>();
            entityCommandHandler.BindCommand<StopAnimCommand, AnimationSystem>();
        }

        #endregion Public Methods
    }
}