﻿using OpenBreed.Core;
using OpenBreed.Components.Common;
using OpenBreed.Rendering.Interface;
using OpenBreed.Ecsw.Components;

namespace OpenBreed.Components.Rendering
{
    public interface ISpriteComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        string AtlasName { get; set; }
        int ImageIndex { get; set; }
        int Order { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    /// Axis-aligned sprite render component
    /// Shared components:
    ///  - axis-aligned box shape
    ///  - position
    /// </summary>
    public class SpriteComponent : IEntityComponent
    {
        #region Internal Constructors

        internal SpriteComponent(SpriteComponentBuilderEx builder)
        {
            AtlasId = builder.AtlasId;
            ImageId = builder.ImageId;
            Order = builder.Order;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Id of sprite atlas
        /// </summary>
        public int AtlasId { get; set; }

        /// <summary>
        /// Id of sprite image from the atlas
        /// </summary>
        public int ImageId { get; set; }

        /// <summary>
        /// Order of drawing, higher value object is rendered on top of lower value objects
        /// </summary>
        public float Order { get; set; }

        #endregion Public Properties
    }

    public sealed class SpriteComponentFactory : ComponentFactoryBase<ISpriteComponentTemplate>
    {
        #region Public Constructors

        public SpriteComponentFactory(ICore core) : base(core)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(ISpriteComponentTemplate template)
        {
            var builder = SpriteComponentBuilderEx.New(core);
            builder.SetAtlasByName(template.AtlasName);
            builder.SetImageId(template.ImageIndex);
            builder.SetOrder(template.Order);
            return builder.Build();
        }

        #endregion Protected Methods
    }

    public class SpriteComponentBuilderEx
    {
        #region Private Fields

        private ICore core;

        #endregion Private Fields

        #region Private Constructors

        private SpriteComponentBuilderEx(ICore core)
        {
            this.core = core;
        }

        #endregion Private Constructors

        #region Internal Properties

        internal int AtlasId { get; private set; }
        internal int ImageId { get; private set; }
        internal float Order { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public static SpriteComponentBuilderEx New(ICore core)
        {
            return new SpriteComponentBuilderEx(core);
        }

        public SpriteComponent Build()
        {
            return new SpriteComponent(this);
        }

        public void SetOrder(int order)
        {
            Order = order;
        }

        public void SetImageId(int imageId)
        {
            ImageId = imageId;
        }

        public void SetAtlasByName(string atlasName)
        {
            AtlasId = core.GetModule<IRenderModule>().Sprites.GetByName(atlasName).Id;
        }

        #endregion Public Methods
    }
}