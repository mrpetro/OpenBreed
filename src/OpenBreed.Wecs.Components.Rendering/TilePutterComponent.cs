using OpenBreed.Common.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Components.Rendering
{
    public interface ITileDataTemplate
    {
        /// <summary>
        /// Name of tile atlas
        /// </summary>
        string AtlasName { get; set; }

        /// <summary>
        /// Tile index in tile atlas
        /// </summary>
        int ImageIndex { get; set; }

        /// <summary>
        /// Tile postion
        /// </summary>
        Vector2 Position { get; set; }
    }

    public interface ITilePutterComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        /// <summary>
        /// Tiles to put
        /// </summary>
        IEnumerable<ITileDataTemplate> Items { get; }

        #endregion Public Properties
    }

    public class TileData
    {
        #region Public Constructors

        public TileData(int atlasId, int imageIndex, int layerNo, Vector2 position)
        {
            AtlasId = atlasId;
            ImageIndex = imageIndex;
            LayerNo = layerNo;
            Position = position;
        }

        #endregion Public Constructors

        #region Public Properties

        public int AtlasId { get; }
        public int ImageIndex { get; }
        public int LayerNo { get; }
        public Vector2 Position { get; }

        #endregion Public Properties
    }

    public class TilePutterComponent : IEntityComponent
    {
        #region Public Constructors

        public TilePutterComponent(TilePutterComponentBuilder builder)
        {
            Items = builder.Items;
        }

        public TilePutterComponent()
        {
            Items = new List<TileData>();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<TileData> Items { get; }

        #endregion Public Properties
    }

    public class TileDataBuilder
    {
        private ITileMan tileMan;

        public TileDataBuilder(ITileMan tileMan)
        {
            this.tileMan = tileMan;
        }

        public int AtlasId { get; private set; }
        public int ImageIndex { get; private set; }
        public Vector2 Position { get; private set; }

        public void SetAtlasByName(string value)
        {
            var tileAtlas = tileMan.GetByName(value);

            if (tileAtlas is null)
                throw new InvalidOperationException($"Tile atlas with name '{value}' is not loaded.");

            AtlasId = tileMan.GetByName(value).Id;
        }

        public void SetImageIndex(int value)
        {
            ImageIndex = value;
        }

        public void SetPosition(Vector2 value)
        {
            Position = value;
        }

        public TileData Build()
        {
            return new TileData(AtlasId, ImageIndex, 0, Position);
        }
    }

    public class TilePutterComponentBuilder : IBuilder<TilePutterComponent>
    {
        #region Private Fields

        private readonly ITileMan tileMan;

        #endregion Private Fields

        #region Internal Constructors

        internal TilePutterComponentBuilder(ITileMan tileMan)
        {
            this.tileMan = tileMan;
            Items = new List<TileData>();
        }

        internal List<TileData> Items { get; }

        #endregion Internal Constructors

        #region Public Methods

        public TilePutterComponent Build()
        {
            return new TilePutterComponent(this);
        }

        public TileDataBuilder CreateData()
        {
            return new TileDataBuilder(tileMan);
        }

        public void AddData(TileData data)
        {
            Items.Add(data);
        }

        #endregion Public Methods
    }

    public sealed class TilePutterComponentFactory : ComponentFactoryBase<ITilePutterComponentTemplate>
    {
        #region Private Fields

        private readonly IBuilderFactory builderFactory;

        #endregion Private Fields

        #region Public Constructors

        public TilePutterComponentFactory(IBuilderFactory builderFactory)
        {
            this.builderFactory = builderFactory;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(ITilePutterComponentTemplate template)
        {
            var tilePutterBuilder = builderFactory.GetBuilder<TilePutterComponentBuilder>();

            var dataBuilder = tilePutterBuilder.CreateData();

            foreach (var item in template.Items)
            {
                dataBuilder.SetAtlasByName(item.AtlasName);
                dataBuilder.SetImageIndex(item.ImageIndex);
                dataBuilder.SetPosition(item.Position);

                tilePutterBuilder.AddData(dataBuilder.Build());
            }

            return tilePutterBuilder.Build();
        }

        #endregion Protected Methods
    }
}