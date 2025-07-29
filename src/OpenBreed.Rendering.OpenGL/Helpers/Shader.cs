using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    public abstract class Shader
    {
        #region Private Fields

        private Dictionary<string, int> uniformLocations;

        #endregion Private Fields

        #region Protected Constructors

        protected Shader(string vertPath, string fragPath)
        {
            VertPath = vertPath;
            FragPath = fragPath;
        }

        #endregion Protected Constructors

        #region Public Properties

        public int Handle { get; private set; }

        #endregion Public Properties

        #region Internal Properties

        internal string VertPath { get; }
        internal string FragPath { get; }

        #endregion Internal Properties

        #region Internal Methods

        internal void InitUniformProperty(string name, int location)
        {
            var propertyInfo = GetType().GetProperty(name, System.Reflection.BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo is null)
            {
                throw new InvalidOperationException($"Shader has missing public property with name '{name}'.");
            }

            propertyInfo.SetValue(this, location);
        }

        internal int GetLocation(string name)
        {
            return uniformLocations[name];
        }

        internal void Initialize(int handle, Dictionary<string, int> uniformLocations)
        {
            Handle = handle;

            this.uniformLocations = uniformLocations;

            InitUniformProperties();
        }

        #endregion Internal Methods

        #region Private Methods

        private void InitUniformProperties()
        {
            foreach (var pair in uniformLocations)
            {
                InitUniformProperty(pair.Key, pair.Value);
            }
        }

        #endregion Private Methods
    }
}