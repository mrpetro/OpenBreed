using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Physics.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Game.Entities.Builders;

namespace OpenBreed.Game.Entities
{
    public class WorldBlock : WorldEntity
    {
        #region Public Constructors

        public WorldBlock(WorldBlockBuilder builder) : base(builder)
        {
            X = builder.x;
            Y = builder.y;

            var position = new Position(X * 16, Y * 16);
            Components.Add(position);
            Components.Add(new GridBoxBody(16));
            Components.Add(builder.Core.Rendering.CreateTile(builder.tileAtlas, builder.tileId));
        }

        #endregion Public Constructors

        #region Public Properties

        public int X { get; }
        public int Y { get; }

        #endregion Public Properties
    }
}