using OpenBreed.Common;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK;
using System;

namespace OpenBreed.Wecs.Components.Rendering
{
    public interface ISpriteComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        string AtlasName { get; set; }
        int ImageIndex { get; set; }
        Vector2 Origin { get; set; }
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

        internal SpriteComponent(SpriteComponentBuilder builder)
        {
            AtlasId = builder.AtlasId;
            ImageId = builder.ImageId;
            Origin = builder.Origin;
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
        /// Local origin of sprite coordinates
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// Order of drawing, higher value object is rendered on top of lower value objects
        /// </summary>
        public float Order { get; set; }

        #endregion Public Properties
    }

    public sealed class SpriteComponentFactory : ComponentFactoryBase<ISpriteComponentTemplate>
    {
        #region Private Fields

        private readonly IManagerCollection managerCollection;

        #endregion Private Fields

        #region Internal Constructors

        internal SpriteComponentFactory(IManagerCollection managerCollection)
        {
            this.managerCollection = managerCollection;
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(ISpriteComponentTemplate template)
        {
            var builder = managerCollection.GetManager<SpriteComponentBuilder>();
            builder.SetAtlasByName(template.AtlasName);
            builder.SetImageId(template.ImageIndex);
            builder.SetOrigin(template.Origin);
            builder.SetOrder(template.Order);
            return builder.Build();
        }

        #endregion Protected Methods
    }

    public class SpriteComponentBuilder
    {
        #region Private Fields

        private readonly ISpriteMan spriteMan;

        #endregion Private Fields

        #region Internal Constructors

        internal SpriteComponentBuilder(ISpriteMan spriteMan)
        {
            this.spriteMan = spriteMan;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal int AtlasId { get; private set; }
        internal int ImageId { get; private set; }
        internal Vector2 Origin { get; private set; }
        internal float Order { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public SpriteComponent Build()
        {
            return new SpriteComponent(this);
        }

        public void SetOrder(int order)
        {
            Order = order;
        }

        public void SetOrigin(Vector2 origin)
        {
            Origin = origin;
        }

        public void SetImageId(int imageId)
        {
            ImageId = imageId;
        }

        public void SetAtlasByName(string atlasName)
        {
            AtlasId = spriteMan.GetByName(atlasName).Id;
        }

        #endregion Public Methods
    }
}