using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    public class PictureMan : IPictureMan
    {
        #region Private Fields

        private readonly Queue<IPicture> loadQueue = new Queue<IPicture>();
        private readonly List<Picture> items = new List<Picture>();
        private readonly Dictionary<string, Picture> names = new Dictionary<string, Picture>();
        private readonly ITextureMan textureMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public PictureMan(ITextureMan textureMan,
                         ILogger logger)
        {
            this.textureMan = textureMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public IPictureBuilder CreatePicture()
        {
            return new PictureBuilder(this, textureMan);
        }

        public IPicture GetById(int atlasId)
        {
            return InternalGetById(atlasId);
        }

        public bool Contains(string pictureName)
        {
            return names.ContainsKey(pictureName);
        }

        public string GetName(int pictureId)
        {
            //TODO: Very ineffective. Name should be part of ISpriteAtlas object.
            return names.First(pair => pair.Value == items[pictureId]).Key;
        }

        public IPicture GetByName(string pictureName)
        {
            if (names.TryGetValue(pictureName, out Picture result))
                return result;

            return null;
        }

        public void UnloadAll(IRenderContext renderContext)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Internal Methods

        internal Picture InternalGetById(int atlasId)
        {
            return items[atlasId];
        }

        internal int Register(string name, Picture picture)
        {
            items.Add(picture);
            names.Add(name, picture);
            loadQueue.Enqueue(picture);

            logger.LogTrace("Picture '{0}' created with ID {1}.", name, items.Count - 1);

            return items.Count - 1;
        }

        #endregion Internal Methods
    }
}