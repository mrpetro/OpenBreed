using OpenBreed.Common.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Wecs.Components.Rendering
{
    public interface IPictureComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        bool Hidden { get; set; }
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
            Hidden = builder.Hidden;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Flag for making this sprite hidden or not
        /// </summary>
        public bool Hidden { get; set; }

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

        private readonly IPictureMan imageMan;

        #endregion Private Fields

        #region Internal Constructors

        internal PictureComponentBuilder(IPictureMan imageMan)
        {
            this.imageMan = imageMan;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal bool Hidden { get; private set; }
        internal int ImageId { get; private set; }
        internal float Order { get; private set; }
        internal Vector2 Origin { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public PictureComponent Build()
        {
            return new PictureComponent(this);
        }

        public void SetHidden(bool value)
        {
            Hidden = value;
        }

        public void SetImageByName(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
            {
                ImageId = PictureComponent.NoImageId;
                return;
            }

            var image = imageMan.GetByName(imageName);

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

        #endregion Private Fields

        #region Public Constructors

        public PictureComponentFactory(IBuilderFactory builderFactory)
        {
            this.builderFactory = builderFactory;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IPictureComponentTemplate template)
        {
            var builder = builderFactory.GetBuilder<PictureComponentBuilder>();
            builder.SetImageByName(template.ImageName);
            builder.SetOrigin(template.Origin);
            builder.SetOrder(template.Order);
            builder.SetHidden(template.Hidden);
            return builder.Build();
        }

        #endregion Protected Methods
    }
}