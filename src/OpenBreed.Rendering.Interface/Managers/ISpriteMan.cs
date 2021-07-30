﻿using OpenTK;

namespace OpenBreed.Rendering.Interface.Managers
{
    /// <summary>
    /// Sprite manager interface
    /// </summary>
    public interface ISpriteMan
    {
        #region Public Methods

        /// <summary>
        /// Get sprite atlas by it's id
        /// </summary>
        /// <param name="id">Id of sprite atlas to get</param>
        /// <returns>Sprite atlas object</returns>
        ISpriteAtlas GetById(int id);

        /// <summary>
        /// Get sprite atlas by it's name
        /// </summary>
        /// <param name="atlasName">Name of sprite atlas to get</param>
        /// <returns>Sprite atlas object</returns>
        ISpriteAtlas GetByName(string atlasName);

        /// <summary>
        /// Checks if atlas with given name already exists
        /// </summary>
        /// <param name="atlasName">Name of atlas to check</param>
        /// <returns>True if exits, false otherwise</returns>
        bool Contains(string atlasName);

        /// <summary>
        /// Creates new sprite atlas
        /// </summary>
        /// <returns>Sprite atlas builder</returns>
        ISpriteAtlasBuilder CreateAtlas();

        /// <summary>
        /// Unloads all textures
        /// </summary>
        void UnloadAll();

        /// <summary>
        /// Render particular sprite giving it's atlas ID, image ID and position
        /// </summary>
        /// <param name="atlasId">Atlas ID of rendered sprite</param>
        /// <param name="imageId">Image ID of rendered sprite</param>
        /// <param name="clipBox">Clip box to determine if sprite should be drawn or not</param>
        void Render(int atlasId, int imageId, Vector2 pos, float order, Box2 clipBox);

        #endregion Public Methods
    }
}