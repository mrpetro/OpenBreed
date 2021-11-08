using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;

namespace OpenBreed.Wecs.Systems.Core.Extensions
{
    public static class ManagerCollectionExtensions
    {
        #region Public Methods

        public static void SetupCoreSystems(this IManagerCollection manCollection)
        {
            var systemFactory = manCollection.GetManager<ISystemFactory>();
            systemFactory.Register(() => new FsmSystem(manCollection.GetManager<IEntityMan>(),
                                                       manCollection.GetManager<IFsmMan>(),
                                                       manCollection.GetManager<ILogger>()));
            systemFactory.Register(() => new TextInputSystem(manCollection.GetManager<IEntityMan>()));
            systemFactory.Register(() => new TimerSystem(manCollection.GetManager<IEntityMan>(),
                                                         manCollection.GetManager<ILogger>()));
        }

        #endregion Public Methods
    }
}