using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Attributes;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Wecs.Components.Rendering
{
    public interface IPictureComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        Color4 Color { get; set; }
        string ImageName { get; set; }
        int Order { get; set; }
        Vector2 Origin { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    /// Axis-aligned image render component
    /// Shared components:
    ///  - axis-aligned box shape
    ///  - position
    /// </summary>
    [ComponentName("Picture")]
    public class PictureComponent : IEntityComponent
    {
        #region Public Fields

        public const int NoImageId = -1;

        #endregion Public Fields

        #region Internal Constructors

        internal PictureComponent(PictureComponentBuilder builder)
        {
            ImageId = builder.ImageId;
            Origin = builder.Origin;
            Order = builder.Order;
            Color = builder.Color;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Color of this picture
        /// </summary>
        public Color4 Color { get; set; }

        /// <summary>
        /// Id of image from the atlas
        /// </summary>
        public int ImageId { get; set; }

        /// <summary>
        /// Order of drawing, higher value object is rendered on top of lower value objects
        /// </summary>
        public float Order { get; set; }

        /// <summary>
        /// Local origin of image coordinates
        /// </summary>
        public Vector2 Origin { get; set; }

        #endregion Public Properties
    }

    public class PictureComponentBuilder : IBuilder<PictureComponent>
    {
        #region Private Fields

        private readonly IPictureDataLoader pictureDataLoader;

        #endregion Private Fields

        #region Internal Constructors

        internal PictureComponentBuilder(
            IPictureDataLoader pictureDataLoader)
        {
            this.pictureDataLoader = pictureDataLoader;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal Color4 Color { get; private set; }
        internal int ImageId { get; private set; }
        internal float Order { get; private set; }
        internal Vector2 Origin { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public PictureComponent Build()
        {
            return new PictureComponent(this);
        }

        public void SetColor(Color4 value)
        {
            Color = value;
        }

        public void SetImageByName(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
            {
                ImageId = PictureComponent.NoImageId;
                return;
            }

            var image = pictureDataLoader.Load(imageName);

            if (image is null)
                throw new InvalidOperationException($"Image with name '{imageName}' is not loaded.");

            ImageId = image.Id;
        }

        public void SetOrder(int order)
        {
            Order = order;
        }

        public void SetOrigin(Vector2 origin)
        {
            Origin = origin;
        }

        #endregion Public Methods
    }

    public sealed class PictureComponentFactory : ComponentFactoryBase<IPictureComponentTemplate>
    {
        #region Private Fields

        private readonly IBuilderFactory builderFactory;
        private readonly IDataLoaderFactory dataLoaderFactory;

        #endregion Private Fields

        #region Public Constructors

        public PictureComponentFactory(
            IBuilderFactory builderFactory,
            IDataLoaderFactory dataLoaderFactory)
        {
            this.builderFactory = builderFactory;
            this.dataLoaderFactory = dataLoaderFactory;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IPictureComponentTemplate template)
        {
            var pictureDataLoader = dataLoaderFactory.GetLoader<IPictureDataLoader>();

            pictureDataLoader.Load(template.ImageName);

            var builder = builderFactory.GetBuilder<PictureComponentBuilder>();
            builder.SetImageByName(template.ImageName);
            builder.SetOrigin(template.Origin);
            builder.SetOrder(template.Order);
            builder.SetColor(template.Color);
            return builder.Build();
        }

        #endregion Protected Methods
    }
}