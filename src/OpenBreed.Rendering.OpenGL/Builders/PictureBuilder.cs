using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Builders
{
    internal class PictureBuilder : IPictureBuilder
    {
        #region Internal Fields

        internal readonly List<Box2> bounds = new List<Box2>();

        internal Vector2 offset = Vector2.Zero;

        #endregion Internal Fields

        #region Private Fields

        private readonly ITextureMan textureMan;
        private readonly PictureMan pictureMan;

        #endregion Private Fields

        #region Internal Constructors

        internal PictureBuilder(PictureMan pictureMan, ITextureMan textureMan)
        {
            this.pictureMan = pictureMan;
            this.textureMan = textureMan;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal ITexture Texture { get; private set; }

        internal string Name { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public IPictureBuilder SetName(string name)
        {
            if (pictureMan.Contains(name))
                throw new InvalidOperationException($"Picture with name '{name}' already exists.");

            Name = name;
            return this;
        }

        public IPictureBuilder SetTexture(int textureId)
        {
            var texture = textureMan.GetById(textureId);

            if (texture is null)
                throw new InvalidOperationException($"Texture with ID '{textureId}' not found.");

            Texture = texture;

            return this;
        }

        public void SetTexture(ITexture texture)
        {
            Texture = texture;
        }

        public IPicture Build()
        {
            return new Picture(this);
        }

        #endregion Public Methods

        #region Internal Methods

        internal int GetVbo()
        {
            return pictureMan.CreateVertices(new UvBox(0, 0, Texture.Width, Texture.Height), Texture.Width, Texture.Height);
        }

        internal int Register(Picture picture)
        {
            return pictureMan.Register(Name, picture);
        }

        #endregion Internal Methods
    }
}