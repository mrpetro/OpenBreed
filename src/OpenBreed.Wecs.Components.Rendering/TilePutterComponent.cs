using OpenBreed.Common;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK;
using System;

namespace OpenBreed.Wecs.Components.Rendering
{
    public interface ITilePutterComponentTemplate : IComponentTemplate
    {
        #region Public Properties

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

        #endregion Public Properties
    }

    public class TilePutterComponent : IEntityComponent
    {
        #region Public Constructors

        public TilePutterComponent(TilePutterComponentBuilder builder)
        {
            AtlasId = builder.AtlasId;
            ImageIndex = builder.ImageIndex;
            Position = builder.Position;
        }

        public TilePutterComponent(int atlasId, int imageIndex, int layerNo, Vector2 position)
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

    public sealed class TilePutterComponentFactory : ComponentFactoryBase<ITilePutterComponentTemplate>
    {
        #region Private Fields

        private readonly IBuilderFactory builderFactory;

        #endregion Private Fields

        #region Internal Constructors

        public TilePutterComponentFactory(IBuilderFactory builderFactory)
        {
            this.builderFactory = builderFactory;
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(ITilePutterComponentTemplate template)
        {
            var builder = builderFactory.GetBuilder<TilePutterComponentBuilder>();
            builder.SetAtlasByName(template.AtlasName);
            builder.SetImageIndex(template.ImageIndex);
            builder.SetPosition(template.Position);
            return builder.Build();
        }

        #endregion Protected Methods
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
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal int AtlasId { get; private set; }
        internal int ImageIndex { get; private set; }
        internal Vector2 Position { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public TilePutterComponent Build()
        {
            return new TilePutterComponent(this);
        }

        public void SetPosition(Vector2 value)
        {
            Position = value;
        }

        public void SetImageIndex(int value)
        {
            ImageIndex = value;
        }

        public void SetAtlasByName(string value)
        {
            var tileAtlas = tileMan.GetByName(value);

            if (tileAtlas is null)
                throw new InvalidOperationException($"Tile atlas with name '{value}' is not loaded.");

            AtlasId = tileMan.GetByName(value).Id;
        }

        #endregion Public Methods
    }
}