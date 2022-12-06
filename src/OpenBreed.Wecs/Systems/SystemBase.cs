using OpenBreed.Common;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenBreed.Wecs.Systems
{
    public abstract class SystemBase<TSystem> : SystemBase where TSystem : ISystem
    {
        private static readonly List<Type> requiredComponentTypes = new List<Type>();

        private static readonly List<Type> forbiddenComponentTypes = new List<Type>();

        static SystemBase()
        {
            var systemType = typeof(TSystem);

            var attributes = systemType.GetCustomAttributes(inherit: true);

            for (int i = 0; i < attributes.Length; i++)
            {
                switch (attributes[i])
                {
                    case RequireEntityWithAttribute requireEntityWithAttribute:
                        foreach (var componentType in requireEntityWithAttribute.ComponentTypes)
                            RequireEntityWith(componentType);
                        break;
                    case RequireEntityWithoutAttribute requireEntityWithoutAttribute:
                        foreach (var componentType in requireEntityWithoutAttribute.ComponentTypes)
                            RequireEntityWithout(componentType);
                        break;
                    default:
                        break;
                }
            }
        }

        protected SystemBase(IWorld world) : base(world)
        {
        }


        public override bool Matches(IEntity entity)
        {
            foreach (var type in forbiddenComponentTypes)
            {
                if (entity.ComponentTypes.Any(item => type.IsAssignableFrom(item)))
                    return false;
            }

            foreach (var type in requiredComponentTypes)
            {
                if (!entity.ComponentTypes.Any(item => type.IsAssignableFrom(item)))
                    return false;
            }

            return true;
        }

        //protected static int RequireEntityWith<TComponent>() where TComponent : IEntityComponent
        //{
        //    return RequireEntityWith(typeof(TComponent));
        //}

        //protected static int RequireEntityWithout<TComponent>() where TComponent : IEntityComponent
        //{
        //    return RequireEntityWithout(typeof(TComponent));
        //}

        protected static int RequireEntityWith(Type componentType)
        {
            var typeIndex = requiredComponentTypes.IndexOf(componentType);

            if (typeIndex >= 0)
                return typeIndex;
            else
            {
                requiredComponentTypes.Add(componentType);
                return requiredComponentTypes.Count - 1;
            }
        }

        protected static int RequireEntityWithout(Type componentType)
        {
            var typeIndex = forbiddenComponentTypes.IndexOf(componentType);

            if (typeIndex >= 0)
                return typeIndex;
            else
            {
                forbiddenComponentTypes.Add(componentType);
                return forbiddenComponentTypes.Count - 1;
            }
        }
    }

    public abstract class SystemBase : ISystem
    {
        #region Private Fields

        private readonly HashSet<IEntity> toAdd = new HashSet<IEntity>();

        private readonly HashSet<IEntity> toRemove = new HashSet<IEntity>();


        #endregion Private Fields

        #region Protected Constructors

        protected SystemBase(IWorld world)
        {
            World = world;

        }

        #endregion Protected Constructors

        #region Public Properties

        /// <summary>
        /// World which owns this system
        /// </summary>
        public IWorld World { get; }

        /// <summary>
        /// Id of the phase in which system will be updated
        /// </summary>
        public int PhaseId { get; }

        #endregion Public Properties

        #region Public Methods

        public abstract bool Matches(IEntity entity);

        public void RequestAddEntity(IEntity entity)
        {
            toAdd.Add(entity);
        }

        public void RequestRemoveEntity(IEntity entity)
        {
            toRemove.Add(entity);
        }

        public void Cleanup()
        {
            if (toRemove.Any())
            {
                //Process entities to remove
                toRemove.ForEach(entity => OnRemoveEntity(entity));
                toRemove.Clear();
            }

            if (toAdd.Any())
            {
                //Process entities to add
                toAdd.ForEach(entity => OnAddEntity(entity));
                toAdd.Clear();
            }
        }

        public bool HasEntity(IEntity entity)
        {
            if (toAdd.Contains(entity))
                return true;

            return ContainsEntity(entity);
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract bool ContainsEntity(IEntity entity);

        protected abstract void OnRemoveEntity(IEntity entity);

        protected abstract void OnAddEntity(IEntity entity);

        #endregion Protected Methods
    }
}