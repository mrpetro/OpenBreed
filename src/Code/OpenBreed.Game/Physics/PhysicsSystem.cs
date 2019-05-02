using OpenBreed.Game.Common;
using OpenBreed.Game.Physics.Components;
using System;

namespace OpenBreed.Game.Physics
{
    public class PhysicsSystem : WorldSystem<IPhysicsComponent>
    {

        #region Private Fields

        private StaticBoxBody[] staticBoxes;

        #endregion Private Fields

        #region Public Constructors

        public PhysicsSystem()
        {
            InitializeStaticBoxes(64, 64);
        }

        #endregion Public Constructors

        #region Public Properties

        public int TileMapHeight { get; private set; }

        public int TileMapWidth { get; private set; }

        #endregion Public Properties

        #region Protected Methods

        protected override void AddComponent(IPhysicsComponent component)
        {
            if (component is StaticBoxBody)
                AddStaticBox((StaticBoxBody)component);
            else
                throw new NotImplementedException($"{component}");
        }

        protected override void RemoveComponent(IPhysicsComponent component)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods

        #region Private Methods


        private void AddStaticBox(StaticBoxBody box)
        {
            int x, y;
            box.GetMapIndices(out x, out y);
            var tileId = x + TileMapHeight * y;

            if (x >= TileMapWidth)
                throw new InvalidOperationException($"Tile X coordinate exceeds tile map width size.");

            if (y >= TileMapHeight)
                throw new InvalidOperationException($"Tile Y coordinate exceeds tile map height size.");

            staticBoxes[tileId] = box;
        }
        private void InitializeStaticBoxes(int width, int height)
        {
            TileMapHeight = width;
            TileMapWidth = height;
            staticBoxes = new StaticBoxBody[width * height];
        }

        #endregion Private Methods

    }
}