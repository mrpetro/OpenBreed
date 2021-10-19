using OpenBreed.Common.Logging;
using OpenBreed.Common.Tools.Collections;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Generic.Shapes;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using System;
using System.Collections;
using System.Diagnostics;

namespace OpenBreed.Physics.Generic.Managers
{
    internal class ShapeMan : IShapeMan
    {
        #region Private Fields

        private readonly IdMap<IShape> items = new IdMap<IShape>();
        private readonly Hashtable tagsToIds = new Hashtable();
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public ShapeMan(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public int GetIdByTag(string tag)
        {
            return (int)tagsToIds[tag];
        }

        public IShape GetByTag(string tag)
        {
            var id = tagsToIds[tag];

            if (id == null)
                return null;

            return items[(int)id];
        }

        public IShape GetById(int id)
        {
            if (items.TryGetValue(id, out IShape entity))
                return entity;
            else
                return null;
        }

        public void Register(string tag, IShape shape, bool overwrite = false)
        {
            if (tagsToIds.ContainsKey(tag))
            {
                if (overwrite)
                    logger.Warning($"Overwriting existing shape under tag '{tag}'.");
                else
                    throw new InvalidOperationException($"Shape already registered under tag '{tag}'.");
            }

            tagsToIds[tag] = items.Add(shape);
        }

        #endregion Public Methods
    }
}