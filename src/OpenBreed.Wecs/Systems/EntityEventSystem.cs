using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems
{
    public abstract class EntityEventSystem<TSystem, TEntityEvent> : SystemBase<TSystem>, IEventSystem<TEntityEvent> where TSystem : ISystem where TEntityEvent :EntityEvent
    {
        #region Protected Fields

        protected readonly HashSet<IEntity> entities = new HashSet<IEntity>();

        #endregion Protected Fields

        #region Private Fields

        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Protected Constructors

        protected EntityEventSystem(IEventsMan eventsMan)
        {
            this.eventsMan = eventsMan;

            eventsMan.Subscribe<TEntityEvent>(Update);
        }

        #endregion Protected Constructors

        #region Public Methods

        public override void AddEntity(IEntity entity) => entities.Add(entity);

        public override bool ContainsEntity(IEntity entity) => entities.Contains(entity);

        public override void RemoveEntity(IEntity entity) => entities.Remove(entity);

        public void Update(object sender, TEntityEvent e)
        {
            foreach (var entity in entities)
            {
                UpdateEntity(entity, e);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract void UpdateEntity(IEntity entity, TEntityEvent e);

        #endregion Protected Methods
    }
}
