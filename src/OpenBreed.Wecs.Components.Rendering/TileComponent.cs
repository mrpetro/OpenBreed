﻿using OpenBreed.Core;
using OpenBreed.Rendering.Interface;
using System;
using OpenBreed.Wecs.Components;
using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Wecs.Components.Rendering
{
    /// <summary>
    /// Axis-aligned tile render component with same height and width
    /// </summary>
    public class TileComponent : IEntityComponent
    {
        #region Internal Constructors

        internal TileComponent(TileComponentBuilder builder)
        {
            AtlasId = builder.AtlasId;
            ImageId = builder.ImageId;
            Order = builder.Order;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Id of tile atlas
        /// </summary>
        public int AtlasId { get; set; }

        /// <summary>
        /// Id of tile image from the atlas
        /// </summary>
        public int ImageId { get; set; }

        /// <summary>
        /// Order of drawing, higher value object is rendered on top of lower value objects
        /// </summary>
        public float Order { get; set; }

        public bool IsEmpty { get { return ImageId == 0 && AtlasId == 0; } }

        #endregion Public Properties
    }

    public class TileComponentBuilder
    {
        #region Private Fields

        internal int AtlasId;
        internal int ImageId;
        internal float Order;

        #endregion Private Fields

        #region Private Constructors

        private readonly ICore core;

        private TileComponentBuilder(ICore core)
        {
            this.core = core;
        }

        #endregion Private Constructors

        #region Public Methods

        public static TileComponentBuilder New(ICore core)
        {
            return new TileComponentBuilder(core);
        }

        public TileComponent Build()
        {
            return new TileComponent(this);
        }

        public void SetAtlasById(int atlasId)
        {
            AtlasId = atlasId;
        }

        public void SetAtlasByName(string atlasName)
        {
            var atlas = core.GetManager<ITileMan>().GetByAlias(atlasName);

            SetAtlasById(atlas.Id);
        }

        public void SetImageIndex(int imageIndex)
        {
            ImageId = imageIndex;
        }

        #endregion Public Methods
    }

}