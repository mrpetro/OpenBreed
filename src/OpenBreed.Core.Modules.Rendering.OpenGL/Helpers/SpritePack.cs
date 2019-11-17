using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    /// <summary>
    /// Package of entity and sprite
    /// </summary>
    internal class SpritePack
    {
        #region Internal Constructors

        internal SpritePack(IEntity entity,
            ISpriteComponent sprite,
            Position position)
        {
            Entity = entity;
            Sprite = sprite;
            Position = position;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal IEntity Entity { get; }
        internal ISpriteComponent Sprite { get; }
        internal Position Position { get; }

        #endregion Internal Properties
    }
}