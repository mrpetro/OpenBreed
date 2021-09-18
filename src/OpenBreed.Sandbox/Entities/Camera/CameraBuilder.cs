using OpenBreed.Common;
using OpenBreed.Wecs.Components.Animation;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Builders;
using OpenTK;

namespace OpenBreed.Sandbox.Entities.Camera
{
    public class CameraBuilder : EntityBuilder
    {
        #region Internal Fields

        internal Vector2 position;
        internal float rotation;
        internal float width;
        internal float height;

        #endregion Internal Fields

        #region Private Fields

        private readonly IBuilderFactory builderFactory;

        #endregion Private Fields

        #region Public Constructors

        public CameraBuilder(IEntityMan entityMan, IBuilderFactory builderFactory) : base(entityMan)
        {
            this.builderFactory = builderFactory;
        }

        #endregion Public Constructors

        #region Public Methods

        public override Entity Build()
        {
            var entity = entityMan.Create();
            entity.Add(PositionComponent.Create(position));

            var animationComponentBuilder = builderFactory.GetBuilder<AnimationComponentBuilder>();
            var cameraComponentBuilder = builderFactory.GetBuilder<CameraComponentBuilder>();

            animationComponentBuilder.AddState().SetSpeed(10.0f)
                                     .SetLoop(false)
                                     .SetById(-1);

            entity.Add(animationComponentBuilder.Build());

            cameraComponentBuilder.SetSize(width, height);
            entity.Add(cameraComponentBuilder.Build());
            entity.Add(new PauseImmuneComponent());
            return entity;
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public void SetRotation(float rotation)
        {
            this.rotation = rotation;
        }

        public void SetFov(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        #endregion Public Methods
    }
}