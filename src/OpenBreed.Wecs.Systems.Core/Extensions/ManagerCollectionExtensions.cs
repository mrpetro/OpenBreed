using OpenBreed.Common;
using OpenBreed.Common.Logging;
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
            systemFactory.Register(() => new FsmSystem(manCollection.GetManager<IFsmMan>(),
                                                       manCollection.GetManager<ILogger>()));
            systemFactory.Register(() => new TextInputSystem());
            systemFactory.Register(() => new TimerSystem(manCollection.GetManager<IEntityMan>()));
        }

        #endregion Public Methods
    }
}