using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenBreed.Game.States
{
    public class MenuState : BaseState
    {
        #region Public Fields

        public const string Id = "MENU";

        #endregion Public Fields

        #region Public Properties

        public override string Name { get { return Id; } }

        #endregion Public Properties

        #region Public Methods

        public override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

            GL.Begin(PrimitiveType.Triangles);

            GL.Color3(1.0f, 1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 4.0f);
            GL.Color3(1.0f, 0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 4.0f);
            GL.Color3(0.2f, 0.9f, 1.0f); GL.Vertex3(0.0f, 1.0f, 4.0f);

            GL.End();
        }

        public override void OnUpdate(FrameEventArgs e)
        {
            base.OnUpdate(e);
        }

        public override void ProcessInputs(FrameEventArgs e)
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Key.Escape))
                ChangeState(GameState.Id);

            if(state.IsKeyDown(Key.X))
                StateMan.Program.Exit();
        }

        #endregion Public Methods
    }
}
