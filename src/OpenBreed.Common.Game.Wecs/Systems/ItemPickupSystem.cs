using OpenBreed.Common.Game.Wecs.Events;


namespace OpenBreed.Common.Game.Wecs.Systems
{
    public class ItemPickupSystem : IEventSystem<ActorCollisionEvent>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;

        #endregion Private Fields

        #region Public Constructors

        public ItemPickupSystem(IEntityMan entityMan)
        {
            this.entityMan = entityMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnEvent(ActorCollisionEvent e, IWorld world)
        {
            var actorEntity = entityMan.GetById(e.EntityId);
            var otherEntity = entityMan.GetById(e.OtherEntityId);
        }

        #endregion Public Methods
    }
}