using OpenBreed.Core.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems;
using System;
using System.Collections.Generic;
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

        public const int NO_WORLD = -1;

        public const float MAX_TIME_MULTIPLIER = 10.0f;

        #endregion Public Fields

        #region Private Fields

        private readonly Dictionary<Type, object> modules = new Dictionary<Type, object>();
        private readonly HashSet<Entity> entities = new HashSet<Entity>();
        private readonly HashSet<Entity> toAdd = new HashSet<Entity>();
        private readonly HashSet<Entity> toRemove = new HashSet<Entity>();
        private readonly HashSet<Entity> toAddUpdate = new HashSet<Entity>();
        private readonly HashSet<Entity> toRemoveUpdate = new HashSet<Entity>();
        private float timeMultiplier = 1.0f;

        private IWorldMan worldMan;

        #endregion Private Fields

        #region Internal Constructors

        internal World(WorldBuilder builder)
        {
            Name = builder.name;
            modules = builder.modules;
            Systems = builder.systems.Values.ToArray();
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

        public IEnumerable<Entity> Entities => entities;

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

        public T GetSystem<T>() where T : ISystem
        {
            return Systems.OfType<T>().FirstOrDefault();
        }

        public TModule GetModule<TModule>()
        {
            if (modules.TryGetValue(typeof(TModule), out object module))
                return (TModule)module;
            else
                throw new InvalidOperationException($"Module of type '{typeof(TModule)}' not found.");
        }

        public void Pause() => SetPause(true);

        public void Unpause() => SetPause(false);

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// Method will add given entity to this world.
        /// Entity will not be added immediately but at the end of each world update.
        /// An exception will be thrown if given entity already exists in world
        /// </summary>
        /// <param name="entity">Entity to be added to this world</param>
        internal void RequestAddEntity(Entity entity)
        {
            if (entity.WorldId != NO_WORLD)
                throw new InvalidOperationException("Entity can't exist in more than one world.");

            toAdd.Add(entity);
        }

        /// <summary>
        /// Method will remove given entity from this world.
        /// Entity will not be removed immediately but at the end of each world update.
        /// An exception will be thrown if given entity does not exist in this world.
        /// </summary>
        /// <param name="entity">Entity to be removed from this world</param>
        internal void RequestRemoveEntity(Entity entity)
        {
            if (entity.WorldId != Id)
                throw new InvalidOperationException("Entity doesn't exist in this world");

            toRemove.Add(entity);
        }

        internal void UpdateRemove(Entity entity, Type componentType)
        {
            toRemoveUpdate.Add(entity);
        }

        internal void UpdateAdd(Entity entity, Type componentType)
        {
            toAddUpdate.Add(entity);
        }

        internal void AddToMatchingSystems(Entity entity)
        {
            foreach (var system in Systems)
            {
                if (system.HasEntity(entity))
                    continue;

                if (system.Matches(entity))
                    system.AddEntity(entity);
            }
        }

        internal void RemoveFromNonMatchingSystems(Entity entity)
        {
            foreach (var system in Systems)
            {
                if (!system.HasEntity(entity))
                    continue;

                if (!system.Matches(entity))
                    system.RemoveEntity(entity);
            }
        }

        internal void Initialize(IWorldMan worldMan)
        {
            this.worldMan = worldMan;

            worldMan.RaiseEvent(new WorldInitializedEventArgs(Id));
        }

        internal void RemovePendingEntities()
        {
            //Perform deinitialization of removed entities
            toRemove.ForEach(item => DeinitializeEntity(worldMan, item));
            toRemove.Clear();

            if (toRemoveUpdate.Count > 0)
                Console.WriteLine($"ToRemove: {toRemoveUpdate.Count}");

            toRemoveUpdate.ForEach(item => RemoveFromNonMatchingSystems(item));
            toRemoveUpdate.Clear();

            //Perform cleanup on all world systems
            Systems.ForEach(item => item.Cleanup());
        }

        internal void InitializeSystems()
        {
            for (int i = 0; i < Systems.Length; i++)
                Systems[i].Initialize(this);
        }

        internal void Update(float dt)
        {
            AddPendingEntities();

            if (Paused)
            {
                foreach (var item in Systems.OfType<IUpdatableSystem>())
                {
                    item.UpdatePauseImmuneOnly(dt * TimeMultiplier);
                }
            }
            else
            {
                foreach (var item in Systems.OfType<IUpdatableSystem>())
                {
                    item.Update(dt * TimeMultiplier);
                }
            }

            RemovePendingEntities();
        }

        #endregion Internal Methods

        #region Private Methods

        private void AddPendingEntities()
        {
            //Perform initialization of added entities
            toAdd.ForEach(item => InitializeEntity(worldMan, item));
            toAdd.Clear();

            if (toAddUpdate.Count > 0)
                Console.WriteLine($"ToAddUpate: {toAddUpdate.Count}");

            toAddUpdate.ForEach(item => AddToMatchingSystems(item));
            toAddUpdate.Clear();
        }

        private void SetPause(bool paused)
        {
            if (Paused == paused)
                return;

            Paused = paused;

            if (Paused)
                worldMan.RaiseEvent(new WorldPausedEventArgs(Id));
            else
                worldMan.RaiseEvent(new WorldUnpausedEventArgs(Id));
        }

        private void DeinitializeEntity(IWorldMan worldMan, Entity entity)
        {
            RemoveFromAllSystems(entity);
            entities.Remove(entity);
            entity.WorldId = NO_WORLD;
            OnEntityRemoved(worldMan, entity);
        }

        private void InitializeEntity(IWorldMan worldMan, Entity entity)
        {
            AddEntityToSystems(entity);
            entities.Add(entity);
            entity.WorldId = Id;
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

        private void RemoveFromAllSystems(Entity entity)
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