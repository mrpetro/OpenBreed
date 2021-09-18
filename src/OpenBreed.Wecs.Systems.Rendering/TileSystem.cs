using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Core;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs;
using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Wecs.Systems.Rendering
{
    public class TileSystem : SystemBase, IRenderableSystem
    {
        #region Public Fields

        public const int TILE_SIZE = 16;
        private readonly IEntityMan entityMan;
        private readonly ITileMan tileMan;
        private readonly ITileGridMan tileGridMan;
        private readonly IStampMan stampMan;
        public int MAX_TILES_COUNT = 1024 * 1024;
        private readonly int gridId;

        #endregion Public Fields

        #region Private Fields

        private Hashtable entities = new Hashtable();

        #endregion Private Fields

        #region Public Constructors

        internal TileSystem(IEntityMan entityMan, ITileMan tileMan, ITileGridMan tileGridMan, IStampMan stampMan)
        {
            this.entityMan = entityMan;
            this.tileMan = tileMan;
            this.tileGridMan = tileGridMan;
            this.stampMan = stampMan;
            RequireEntityWith<TileComponent>();
            RequireEntityWith<PositionComponent>();

            RegisterHandler<TileSetCommand>(HandleTileSetCommand);
            RegisterHandler<PutStampCommand>(HandlePutStampCommand);

            gridId = tileGridMan.CreateGrid(128, 128, 1, 16);
        }

        #endregion Public Constructors

        #region Public Properties

        #endregion Public Properties

        #region Public Methods

        public void Render(Box2 clipBox, int depth, float dt)
        {
            ExecuteCommands();

            //TODO: Use grid ID from component
            tileGridMan.Render(0, clipBox);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            Debug.Assert(!entities.Contains(entity), "Entity already added!");

            var pos = entity.Get<PositionComponent>();

            var tile = entity.Get<TileComponent>();

            tileGridMan.ModifyTile(0, pos.Value, tile.AtlasId, tile.ImageId);

            entities[entity] = tile;
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods

        #region Private Methods

        private bool HandleTileSetCommand(TileSetCommand cmd)
        {
            var entity = entityMan.GetById(cmd.EntityId);

            tileGridMan.ModifyTile(0, cmd.Position, cmd.AtlasId, cmd.ImageId);

            return true;
        }

        private bool HandlePutStampCommand(PutStampCommand cmd)
        {
            tileGridMan.ModifyTiles(0, cmd.Position, cmd.StampId);

            return true;
        }

        #endregion Private Methods
    }
}