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

        private readonly Dictionary<IEntity, HashSet<IMatchingSystem>> entities = new Dictionary<IEntity, HashSet<IMatchingSystem>>();
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

        public IEnumerable<IEntity> Entities => entities.Keys;

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

        public void RemoveEntity(IEntity entity)
        {
            RemoveFromAllSystems(entity);
            entities.Remove(entity);

            if (((Entity)entity).WorldId == Id)
            {
                ((Entity)entity).WorldId = WecsConsts.NO_WORLD_ID;
            }
        }

        public void AddEntity(IEntity entity)
        {
            var matchingSystems = GetMatchingSystems(entity).ToHashSet();

            entities.Add(entity, matchingSystems);

            foreach (var system in matchingSystems)
                system.AddEntity(entity);

            ((Entity)entity).WorldId = Id;
        }

        private IEnumerable<IMatchingSystem> GetMatchingSystems(IEntity entity)
        {
            foreach (var system in Systems.OfType<IMatchingSystem>())
            {
                if (entityToSystemMatcher.AreMatch(system, entity))
                    yield return system;
            }
        }

        private void RemoveFromAllSystems(IEntity entity)
        {
            if (!entities.TryGetValue(entity, out HashSet<IMatchingSystem> systems))
                throw new InvalidOperationException();

            foreach (var system in systems)
                system.RemoveEntity(entity);
        }

        #endregion Private Methods
    }
}