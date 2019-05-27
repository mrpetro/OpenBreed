//using OpenTK;
//using OpenTK.Graphics.OpenGL;

//namespace OpenBreed.Core.Systems.Rendering.Components
//{
//    public class RenderCharacter
//    {
//        #region Private Fields

//        private float _offset;

//        #endregion Private Fields

//        #region Public Constructors

//        public RenderCharacter(Vector2 position, float charOffset)
//        {
//            _offset = charOffset;
//        }

//        #endregion Public Constructors

//        #region Public Methods

//        public void SetChar(float charOffset)
//        {
//            _offset = charOffset;
//        }

//        public void Draw(IViewport viewport)
//        {
//            GL.VertexAttrib2(2, new Vector2(_offset, 0));
//            var t2 = Matrix4.CreateTranslation(
//                _position.X,
//                _position.Y,
//                _position.Z);
//            var s = Matrix4.CreateScale(_scale);
//            _modelView = s * t2 * viewport.LookAtMatrix;
//            GL.UniformMatrix4(21, false, ref _modelView);
//            _model.Render();
//        }

//        #endregion Public Methods
//    }
//}