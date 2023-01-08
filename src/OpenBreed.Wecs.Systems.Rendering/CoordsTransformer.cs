using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Systems.Rendering
{
    public class CoordsTransformer
    {
        #region Private Fields

        private readonly IEntityMan entityMan;

        private readonly IViewClient viewClient;

        #endregion Private Fields

        #region Public Constructors

        public CoordsTransformer(IEntityMan entityMan, IViewClient viewClient)
        {
            this.entityMan = entityMan;
            this.viewClient = viewClient;
        }

        #endregion Public Constructors

        #region Public Methods

        public Vector2 ClientToWorldEx(Vector2 coords, IEntity viewport)
        {
            var result = ClientToWorld(new Vector4(coords), viewport);

            return new Vector2(result.X, result.Y);
        }

        public Vector4 ClientToWorld(Vector4 coords, IEntity viewport)
        {
            var vpc = viewport.Get<ViewportComponent>();
            var pos = viewport.Get<PositionComponent>();

            var camera = entityMan.GetById(vpc.CameraEntityId);

            var x = Matrix4.Identity;

            //var screenX = viewClient.ClientTransform;
            //x = Matrix4.Mult(screenX, x);

            var viewportX = TransformHelper.GetViewportTransform(pos.Value, vpc.Size);
            x = Matrix4.Mult(viewportX, x);

            var cpos = camera.Get<PositionComponent>();
            var cmc = camera.Get<CameraComponent>();

            var cameraX = TransformHelper.GetCameraTransform(vpc.ScalingType, vpc.Size, cpos.Value, cmc.Size);
            x = Matrix4.Mult(cameraX, x);

            //x.Invert();

            return Vector4.TransformRow(coords, x);
        }

        #endregion Public Methods
    }
}