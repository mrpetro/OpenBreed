using OpenBreed.Common.Tools.Collections;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace OpenBreed.Wecs.Services
{
    internal class EntityClass : IEntityClass
    {
        #region Private Fields

        private HashSet<EntityClass> childs;

        #endregion Private Fields

        #region Public Properties

        public int Id { get; init; }
        public string Name { get; init; }

        public IEntityClass Parent { get; internal set; }

        public IEnumerable<IEntityClass> Childs => childs ?? Enumerable.Empty<IEntityClass>();

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"Entity class '{Name}'";
        }

        #endregion Public Methods

        #region Internal Methods

        internal void AddChild(EntityClass child)
        {
            if (childs is null)
            {
                childs = new HashSet<EntityClass>();
            }

            if (!childs.Add(child))
            {
                throw new InvalidOperationException($"{child} is already a child of {this}.");
            }

            child.Parent = this;
        }

        #endregion Internal Methods
    }

    internal class EntityClassMan : IEntityClassMan
    {
        #region Private Fields

        private readonly IdMap<EntityClass> entityClasses = new IdMap<EntityClass>();
        private readonly Dictionary<string, EntityClass> classByNameLookup = new Dictionary<string, EntityClass>();

        #endregion Private Fields

        #region Public Constructors

        public EntityClassMan()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntityClass RootClass => GetById(0);

        #endregion Public Properties

        #region Public Methods

        public IEntityClass GetByName(string name)
        {
            if (!classByNameLookup.TryGetValue(name, out EntityClass entityClass))
            {
                throw new InvalidOperationException($"Unable to find entity class '{name}'");
            }

            return entityClass;
        }

        public IEntityClass GetById(int classId)
        {
            if (!entityClasses.TryGetValue(classId, out EntityClass entityClass))
            {
                return null;
            }

            return entityClass;
        }

        #endregion Public Methods

        #region Internal Methods

        internal EntityClass GetEntityClass(string className)
        {
            if (classByNameLookup.TryGetValue(className, out EntityClass entityClass))
            {
                throw new InvalidOperationException($"Entity class with name '{className}' already exists.");
            }

            return entityClass;
        }

        internal int CreateClass(string className, string parentClassName = null)
        {
            if (classByNameLookup.ContainsKey(className))
            {
                throw new InvalidOperationException($"Entity class with name '{className}' already exists.");
            }

            var newClassId = entityClasses.NewId();

            var newClass = new EntityClass()
            {
                Id = newClassId,
                Name = className,
            };

            entityClasses.Add(newClass);
            classByNameLookup.Add(className, newClass);

            if (parentClassName is null)
            {
                return newClassId;
            }

            if (!classByNameLookup.TryGetValue(parentClassName, out EntityClass parentClass))
            {
                throw new InvalidOperationException($"Parent entity class with name '{className}' doesn't exist.");
            }

            parentClass.AddChild(newClass);

            return newClassId;
        }

        #endregion Internal Methods
    }
}