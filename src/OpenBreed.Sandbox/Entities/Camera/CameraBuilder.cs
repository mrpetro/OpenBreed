using OpenBreed.Core;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Entities.Builders;
using OpenBreed.Rendering.Components;
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

        #region Public Constructors

        public CameraBuilder(ICore core) : base(core)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override Entity Build()
        {
            var entity = Core.Entities.Create();
            entity.Add(PositionComponent.Create(position));

            var ccBuilder = CameraComponentBuilder.New(Core);
            ccBuilder.SetSize(width, height);
            entity.Add(ccBuilder.Build());
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