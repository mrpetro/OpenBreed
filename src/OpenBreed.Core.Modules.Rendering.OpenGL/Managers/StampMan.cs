using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Rendering.Components.Builders;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Helpers.Builders;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Managers
{
    internal class StampMan : IStampMan
    {
        #region Private Fields

        private readonly List<TileStamp> items = new List<TileStamp>();
        private readonly Dictionary<string, TileStamp> names = new Dictionary<string, TileStamp>();

        #endregion Private Fields

        #region Internal Constructors

        internal StampMan(OpenGLModule module)
        {
            Module = module ?? throw new ArgumentNullException(nameof(module));
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal OpenGLModule Module { get; }

        #endregion Internal Properties

        #region Public Methods

        public IStampBuilder Create()
        {
            return new StampBuilder(this);
        }

        public ITileStamp GetById(int id)
        {
            return items[id];
        }

        public ITileStamp GetByName(string name)
        {
            TileStamp result = null;
            names.TryGetValue(name, out result);
            return result;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Register(string name, TileStamp newStamp)
        {
            items.Add(newStamp);
            names.Add(name, newStamp);
        }

        /// <summary>
        /// Generate ID for new stamp
        /// </summary>
        /// <returns>ID for new stamp</returns>
        internal int GenerateNewId()
        {
            return items.Count;
        }

        #endregion Internal Methods
    }
}