using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Core.Modules.Rendering.Entities
{
    /// <summary>
    /// This class is an camera entity that is part of the world
    ///
    /// </summary>
    public class CameraEntity : Entity
    {
        #region Public Constructors

        public CameraEntity(CameraBuilder builder) : base(builder.Core)
        {
            Position = Common.Systems.Components.Position.Create(builder.position);
            Rotation = builder.rotation;
            Zoom = builder.zoom;

            Add(Position);
            Add(new Camera());
        }

        #endregion Public Constructors

        #region Public Properties

        public IPosition Position { get; set; }

        public float Rotation { get; set; }

        public float Zoom { get; set; }

        #endregion Public Properties

        #region Public Methods

        public Matrix4 GetTransform()
        {
            var transform = Matrix4.Identity;
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-Position.Value.X, -Position.Value.Y, 0));
            transform = Matrix4.Mult(transform, Matrix4.CreateRotationZ(-Rotation));
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(Zoom, Zoom, 1.0f));
            return transform;
        }

        #endregion Public Methods
    }
}