using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenTK;
using OpenBreed.Wecs.Entities.Builders;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs;

namespace OpenBreed.Sandbox.Entities.Camera
{
    public class CameraBuilder : EntityBuilder
    {
        #region Internal Fields

        internal Vector2 position;
        internal float rotation;
        internal float width;
        internal float height;
        private readonly CameraComponentBuilder cameraComponentBuilder;

        #endregion Internal Fields

        #region Public Constructors

        public CameraBuilder(IEntityMan entityMan, CameraComponentBuilder cameraComponentBuilder) : base(entityMan)
        {
            this.cameraComponentBuilder = cameraComponentBuilder;
        }

        #endregion Public Constructors

        #region Public Methods

        public override Entity Build()
        {
            var entity = entityMan.Create();
            entity.Add(PositionComponent.Create(position));

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