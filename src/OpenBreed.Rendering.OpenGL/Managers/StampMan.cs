using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenBreed.Rendering.OpenGL.Helpers;
using System;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    internal class StampMan : IStampMan
    {
        #region Private Fields

        private readonly List<TileStamp> items = new List<TileStamp>();
        private readonly Dictionary<string, TileStamp> names = new Dictionary<string, TileStamp>();

        #endregion Private Fields

        #region Internal Constructors

        internal StampMan()
        {
        }

        #endregion Internal Constructors

        #region Internal Properties

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