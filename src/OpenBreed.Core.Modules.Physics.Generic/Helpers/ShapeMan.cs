using OpenBreed.Core.Collections;
using OpenBreed.Core.Modules.Physics.Shapes;
using System;
using System.Collections;
using System.Diagnostics;

namespace OpenBreed.Core.Modules.Physics.Helpers
{
    public class ShapeMan
    {
        #region Private Fields

        private readonly IdMap<IShape> items = new IdMap<IShape>();
        private readonly Hashtable tagsToIds = new Hashtable();

        #endregion Private Fields

        #region Public Constructors

        public ShapeMan(PhysicsModule module)
        {
            Debug.Assert(module != null);

            Module = module;
        }

        #endregion Public Constructors

        #region Public Properties

        public PhysicsModule Module { get; }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        #endregion Public Properties

        #region Public Methods

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
                    Module.Core.Logging.Warning($"Overwriting existing shape under tag '{tag}'.");
                else
                    throw new InvalidOperationException($"Shape already registered under tag '{tag}'.");
            }

            tagsToIds[tag] = items.Add(shape);
        }

        #endregion Public Methods
    }
}