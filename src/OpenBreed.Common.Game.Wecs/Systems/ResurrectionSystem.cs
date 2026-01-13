using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Core.Systems;

namespace OpenBreed.Common.Game.Wecs.Systems
{
    [RequireEntityWith(
        typeof(ResurrectCommandComponent),
        typeof(ResurrectableComponent))]
    public class ResurrectionSystem : UpdatableMatchingSystemBase
    {
        private readonly IWorldMan worldMan;

        public ResurrectionSystem(IWorldMan worldMan) : base(worldMan)
        {
            this.worldMan = worldMan;
        }

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            try
            {
                var resurectableCmp = entity.Get<ResurrectableComponent>();

                worldMan.RequestAddEntity(entity, resurectableCmp.WorldId);
            }
            finally
            {
                entity.Remove<ResurrectCommandComponent>();
            }
        }
    }
}
