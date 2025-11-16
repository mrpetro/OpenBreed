using OpenBreed.Core.Extensions;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenTK.Mathematics;
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
    internal sealed class World : IWorld
    {
        #region Public Fields

        public const float MAX_TIME_MULTIPLIER = 10.0f;

        #endregion Public Fields

        #region Private Fields

        private readonly Dictionary<IEntity, HashSet<IMatchingSystem>> entitiesToSystemsLookup = new Dictionary<IEntity, HashSet<IMatchingSystem>>();
        private readonly Dictionary<IMatchingSystem, HashSet<IEntity>> systemToEntriesLookup = new Dictionary<IMatchingSystem, HashSet<IEntity>>();

        private readonly IEntityToSystemMatcher entityToSystemMatcher;
        private readonly UpdateContext context;
        private float timeMultiplier = 1.0f;

        #endregion Private Fields

        #region Internal Constructors

        internal World(WorldBuilder builder)
        {
            Name = builder.name;
            entityToSystemMatcher = builder.entityToSystemMatcher;
            context = new UpdateContext();
            Systems = builder.CreateSystems(this).ToArray();
        }

        #endregion Internal Constructors

        #region Public Properties

        public ISystem[] Systems { get; }

        /// <summary>
        /// Time "speed" control value, can't be negative but can be 0 (Basicaly stops time).
        /// </summary>
        public float DtMultiplier
        {
            get
            {
                return timeMultiplier;
            }

            set
            {
                timeMultiplier = MathHelper.Clamp(value, 0, MAX_TIME_MULTIPLIER);
            }
        }

        public IEnumerable<IEntity> Entities => entitiesToSystemsLookup.Keys;

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

        public T GetSystem<T>() where T : IMatchingSystem
        {
            return Systems.OfType<T>().FirstOrDefault();
        }

        public void RemoveEntity(IEntity entity)
        {
            RemoveFromAllSystems(entity);
            entitiesToSystemsLookup.Remove(entity);

            if (((Entity)entity).WorldId == Id)
            {
                ((Entity)entity).WorldId = WecsConsts.NO_WORLD_ID;
            }
        }

        public void AddEntity(IEntity entity)
        {
            var matchingSystems = GetMatchingSystems(entity).ToHashSet();

            entitiesToSystemsLookup.Add(entity, matchingSystems);

            foreach (var system in matchingSystems)
            {
                CacheEntityToSystem(entity, system);

                if (system is IOnAddEntitySystem onAddEntitySystem)
                {
                    onAddEntitySystem.OnAddEntity(this, entity);
                }
            }

            ((Entity)entity).WorldId = Id;
        }

        public IEnumerable<IEntity> GetMatchingEntities(IMatchingSystem system)
        {
            //foreach (var entity in entitiesToSystemsLookup)
            //{
            //    if (entityToSystemMatcher.AreMatch(system, entity.Key))
            //    {
            //        yield return entity.Key;
            //    }
            //}

            if (systemToEntriesLookup.TryGetValue(system, out HashSet<IEntity> entities))
            {
                foreach (var entity in entities)
                {
                    yield return entity;
                }
            }
        }

        public bool HasSystemEntityCached(IMatchingSystem system, IEntity entity)
        {
            if (!systemToEntriesLookup.TryGetValue(system, out HashSet<IEntity> entities))
            {
                return false;
            }

            return entities.Contains(entity);
        }

        public void UpdateSystemsCache(IEntity entity)
        {
            foreach (var system in Systems.OfType<IMatchingSystem>())
            {
                var areMatching = entityToSystemMatcher.AreMatch(system, entity);

                if (HasSystemEntityCached(system, entity))
                {
                    if (!areMatching)
                    {
                        DecacheEntityFromSystem(entity, system);
                        continue;
                    }
                }
                else
                {
                    if (areMatching)
                    {
                        CacheEntityToSystem(entity, system);
                        continue;
                    }
                }
            }
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Update(float dt)
        {
            context.WorldId = Id;
            context.DtMultiplier = DtMultiplier;
            context.UpdateDeltaTime(dt);

            foreach (var item in Systems.OfType<IUpdatableSystem>())
                item.Update(context);
        }

        #endregion Internal Methods

        #region Private Methods

        private void CacheEntityToSystem(IEntity entity, IMatchingSystem system)
        {
            if (!systemToEntriesLookup.TryGetValue(system, out HashSet<IEntity> entities))
            {
                entities = new HashSet<IEntity>();
                systemToEntriesLookup.Add(system, entities);
            }

            entities.Add(entity);
        }

        private void DecacheEntityFromSystem(IEntity entity, IMatchingSystem system)
        {
            if (!systemToEntriesLookup.TryGetValue(system, out HashSet<IEntity> entities))
            {
                return;
            }

            entities.Remove(entity);
        }

        private IEnumerable<IMatchingSystem> GetMatchingSystems(IEntity entity)
        {
            foreach (var system in Systems.OfType<IMatchingSystem>())
            {
                if (entityToSystemMatcher.AreMatch(system, entity))
                {
                    yield return system;
                }
            }
        }

        private void RemoveFromAllSystems(IEntity entity)
        {
            if (!entitiesToSystemsLookup.TryGetValue(entity, out HashSet<IMatchingSystem> systems))
            {
                throw new InvalidOperationException();
            }

            foreach (var system in systems)
            {
                if (system is IOnRemoveEntitySystem onRemoveEntitySystem)
                {
                    onRemoveEntitySystem.OnRemoveEntity(this, entity);
                }

                DecacheEntityFromSystem(entity, system);
            }
        }

        #endregion Private Methods
    }
}