using OpenBreed.Core;
using OpenBreed.Components.Common;
using OpenBreed.Components.Rendering;
using OpenTK;
using OpenBreed.Ecsw.Entities.Builders;
using OpenBreed.Ecsw.Entities;
using OpenBreed.Ecsw;

namespace OpenBreed.Game.Entities
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
            var entity = Core.GetManager<IEntityMan>().Create();
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