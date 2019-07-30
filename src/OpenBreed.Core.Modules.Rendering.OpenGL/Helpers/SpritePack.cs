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
            ISprite sprite,
            IPosition position)
        {
            Entity = entity;
            Sprite = sprite;
            Position = position;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal IEntity Entity { get; }
        internal ISprite Sprite { get; }
        internal IPosition Position { get; }

        #endregion Internal Properties
    }
}