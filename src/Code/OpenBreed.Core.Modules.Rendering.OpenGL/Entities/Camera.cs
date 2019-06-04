using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenBreed.Core.Modules.Rendering.Systems;

namespace OpenBreed.Core.Modules.Rendering.Entities
{
    /// <summary>
    /// This class is an camera entity that is part of the world
    ///
    /// </summary>
    public class Camera : WorldEntity
    {
        #region Public Constructors

        public Camera(CameraBuilder builder) : base(builder)
        {
            Position = builder.position;
            Rotation = builder.rotation;
            Zoom = builder.zoom;

            Transform = new Transformation(builder.position,
                                               builder.rotation,
                                               builder.zoom);
            Components.Add(Transform);
        }

        #endregion Public Constructors

        #region Public Properties

        public Transformation Transform { get; }
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

        public void Update()
        {
        }

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// This will render world part currently visible by the camera into given viewport
        /// </summary>
        /// <param name="viewport">Viewport that camera will render to</param>
        internal void RenderTo(Viewport viewport)
        {
            GL.PushMatrix();

            GL.Translate(viewport.Width / 2, viewport.Height / 2, 0.0f);

            var transform = GetTransform();
            GL.MultMatrix(ref transform);

            //TODO: Design problem, refactor this someday
            var renderSystem = (RenderSystem)CurrentWorld.RenderSystem;

            renderSystem.Draw(viewport);

            GL.PopMatrix();
        }

        #endregion Internal Methods
    }
}