using OpenBreed.Core.Entities;
using OpenBreed.Core.Entities.Builders;
using OpenTK;

namespace OpenBreed.Core.Systems.Rendering.Entities.Builders
{
    public class CameraBuilder : WorldEntityBuilder
    {
        #region Internal Fields

        internal Vector2 position;
        internal float rotation;
        internal float zoom;

        #endregion Internal Fields

        #region Public Constructors

        public CameraBuilder(ICore core) : base(core)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override IEntity Build()
        {
            return new Camera(this);
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public void SetRotation(float rotation)
        {
            this.rotation = rotation;
        }

        public void SetZoom(float zoom)
        {
            this.zoom = zoom;
        }

        #endregion Public Methods
    }
}