using OpenBreed.Game.Entities;
using OpenBreed.Game.Entities.Builders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenBreed.Game.Rendering.Helpers
{
    public class Camera : WorldEntity
    {
        #region Public Constructors

        public Camera(WorldCameraBuilder builder) : base(builder)
        {
            Position = builder.position;
            Rotation = builder.rotation;
            Zoom = builder.zoom;
        }

        //public Camera(Vector2 position, float rotation, float zoom)
        //{
        //    Position = position;
        //    Rotation = rotation;
        //    Zoom = zoom;
        //}

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Position { get; set; }

        public float Rotation { get; set; }

        public float Zoom { get; set; }

        #endregion Public Properties

        #region Public Methods

        public Matrix4 GetTransform()
        {
            var transform = Matrix4.Identity;
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-Position.X, -Position.Y, 0));
            transform = Matrix4.Mult(transform, Matrix4.CreateRotationZ(-Rotation));
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(Zoom, Zoom, 1.0f));
            return transform;
        }

        public void ApplyTransform()
        {
            var transform = GetTransform();
            GL.MultMatrix(ref transform);
        }

        public void Update()
        {
        }

        #endregion Public Methods
    }
}