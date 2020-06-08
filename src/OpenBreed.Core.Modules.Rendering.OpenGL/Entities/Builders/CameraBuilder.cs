using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Entities.Builders;
using OpenBreed.Core.Modules.Rendering.Builders;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenTK;

namespace OpenBreed.Core.Modules.Rendering.Entities.Builders
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

        public override IEntity Build()
        {
            var entity = Core.Entities.Create();
            entity.Add(WorldComponentBuilder.New(Core).Build());
            entity.Add(PositionComponent.Create(position));

            var ccBuilder = CameraComponentBuilder.New(Core);
            ccBuilder.SetProperty("Width", width);
            ccBuilder.SetProperty("Height", height);
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