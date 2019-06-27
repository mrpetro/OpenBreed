using OpenBreed.Core.Systems.Common.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenBreed.Core.Entities
{
    /// <summary>
    /// Entity interface implementation
    /// </summary>
    public class Entity : IEntity
    {

        #region Private Fields

        private EntityMan manager;

        private List<IEntityComponent> components = new List<IEntityComponent>();

        #endregion Private Fields

        #region Public Constructors

        public Entity(EntityMan manager)
        {
            this.manager = manager ?? throw new ArgumentNullException(nameof(manager));

            PerformDelegate = PerformDefault;

            Components = new ReadOnlyCollection<IEntityComponent>(components);

            Guid = Core.Entities.GetGuid();

            Core.Entities.AddEntity(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public ReadOnlyCollection<IEntityComponent> Components { get; }

        public ICore Core { get { return manager.Core; } }

        public World CurrentWorld { get; private set; }

        public Guid Guid { get; }

        public EntityPerform PerformDelegate { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void PostMessage(IEntityMsg message)
        {
            manager.PostMsg(this, message);
        }

        public void Add(IEntityComponent component)
        {
            components.Add(component);
        }

        public virtual void EnterWorld(World world)
        {
            world.AddEntity(this);
        }

        public virtual void LeaveWorld()
        {
            CurrentWorld.RemoveEntity(this);
        }

        public bool Remove(IEntityComponent component)
        {
            return components.Remove(component);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Deinitialize()
        {
            //Deinitialize all entity components
            //for (int i = 0; i < Components.Count; i++)
            //    Components[i].Deinitialize(this);

            //Forget the world in which entity was
            CurrentWorld = null;
        }

        internal void Initialize(World world)
        {
            //Remember in what world entity is
            CurrentWorld = world;

            //Initialize all entity components
            //for (int i = 0; i < Components.Count; i++)
            //    Components[i].Initialize(this);
        }

        #endregion Internal Methods

        #region Private Methods

        private void PerformDefault(string actionName, params object[] arguments)
        {
            //DO NOTHING HERE
        }

        #endregion Private Methods

    }
}