using OpenBreed.Core;
using OpenBreed.Core.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenBreed.Wecs.Worlds
{
    /// <summary>
    /// World class which contains systems and entities
    ///
    /// Enqueues events:
    /// WorldInitializedEvent - when world is initialized
    /// </summary>
    public sealed class World
    {
        #region Public Fields

        public const float MAX_TIME_MULTIPLIER = 10.0f;

        #endregion Public Fields

        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
        private readonly List<Entity> toAdd = new List<Entity>();
        private readonly List<Entity> toRemove = new List<Entity>();
        private float timeMultiplier = 1.0f;

        #endregion Private Fields

        #region Internal Constructors

        internal World(WorldBuilder builder)
        {
            Name = builder.name;
            Systems = builder.systems.Values.ToArray();
            Entities = new ReadOnlyCollection<Entity>(entities);
        }

        #endregion Internal Constructors

        #region Public Properties

        public ISystem[] Systems { get; }

        /// <summary>
        /// Pauses or unpauses this world
        /// </summary>
        public bool Paused { get; internal set; }

        /// <summary>
        /// Time "speed" control value, can't be negative but can be 0 (Basicaly stops time).
        /// </summary>
        public float TimeMultiplier
        {
            get
            {
                return timeMultiplier;
            }

            set
            {
                timeMultiplier = OpenTK.MathHelper.Clamp(value, 0, MAX_TIME_MULTIPLIER);
            }
        }

        public ICore Core { get; }

        public ReadOnlyCollection<Entity> Entities { get; }

        /// <summary>
        /// Id of this world
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// Name of this world
        /// </summary>
        public string Name { get; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"World:{Name}";
        }

        /// <summary>
        /// Method will remove all entities from this world.
        /// Entities will not be removed immediately but at the end of each world update.
        /// </summary>
        public void RemoveAllEntities()
        {
            for (int i = 0; i < entities.Count; i++)
                RemoveEntity(entities[i]);
        }

        public T GetSystem<T>() where T : ISystem
        {
            return Systems.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Method will add given entity to this world.
        /// Entity will not be added immediately but at the end of each world update.
        /// An exception will be thrown if given entity already exists in world
        /// </summary>
        /// <param name="entity">Entity to be added to this world</param>
        public void AddEntity(Entity entity)
        {
            if (entity.World != null)
                throw new InvalidOperationException("Entity can't exist in more than one world.");

            toAdd.Add(entity);
        }

        /// <summary>
        /// Method will remove given entity from this world.
        /// Entity will not be removed immediately but at the end of each world update.
        /// An exception will be thrown if given entity does not exist in this world.
        /// </summary>
        /// <param name="entity">Entity to be removed from this world</param>
        public void RemoveEntity(Entity entity)
        {
            if (entity.World != this)
                throw new InvalidOperationException("Entity doesn't exist in this world");

            toRemove.Add(entity);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Initialize(IWorldMan worldMan)
        {
            //InitializeSystems();
            Cleanup(worldMan);

            worldMan.RaiseEvent(new WorldInitializedEventArgs(Id));
        }

        internal void Cleanup(IWorldMan worldMan)
        {
            //Perform deinitialization of removed entities
            toRemove.ForEach(item => DeinitializeEntity(worldMan, item));

            //Perform cleanup on all world systems
            Systems.ForEach(item => item.Cleanup());

            //Perform initialization of added entities
            toAdd.ForEach(item => InitializeEntity(worldMan, item));

            toRemove.Clear();
            toAdd.Clear();
        }

        internal void InitializeSystems()
        {
            for (int i = 0; i < Systems.Length; i++)
                Systems[i].Initialize(this);
        }

        #endregion Internal Methods

        #region Private Methods

        private void DeinitializeEntity(IWorldMan worldMan, Entity entity)
        {
            ((Entity)entity).World = null;
            entities.Remove(entity);
            RemoveEntityFromSystems(entity);
            OnEntityRemoved(worldMan, entity);
        }

        private void InitializeEntity(IWorldMan worldMan, Entity entity)
        {
            entities.Add(entity);
            ((Entity)entity).World = this;
            AddEntityToSystems(entity);
            OnEntityAdded(worldMan, entity);
        }

        private void OnEntityAdded(IWorldMan worldMan, Entity entity)
        {
            worldMan.RaiseEvent(new EntityAddedEventArgs(Id, entity.Id));
        }

        private void OnEntityRemoved(IWorldMan worldMan, Entity entity)
        {
            worldMan.RaiseEvent(new EntityRemovedEventArgs(Id, entity.Id));
        }

        private void AddEntityToSystems(Entity entity)
        {
            foreach (var system in Systems)
            {
                if (system.Matches(entity))
                    system.AddEntity(entity);
            }
        }

        private void RemoveEntityFromSystems(Entity entity)
        {
            foreach (var system in Systems)
            {
                if (system.Matches(entity))
                    system.RemoveEntity(entity);
            }
        }

        #endregion Private Methods

        //private void InitializeSystems()
        //{
        //    for (int i = 0; i < systems.Count; i++)
        //        systems[i].Initialize(this);
        //}

        //private void DeinitializeSystems()
        //{
        //    for (int i = 0; i < systems.Count; i++)
        //        systems[i].Deinitialize();
        //}
    }
}