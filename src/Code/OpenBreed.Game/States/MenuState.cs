﻿using OpenBreed.Core;
using OpenBreed.Core.States;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenBreed.Game.States
{
    public class MenuState : BaseState
    {
        #region Public Fields

        public const string ID = "MENU";

        #endregion Public Fields

        #region Public Constructors

        public MenuState(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }
        public override string Id { get { return ID; } }

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

            GL.Color3(1.0f, 1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 0.0f);
            GL.Color3(1.0f, 0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 0.0f);
            GL.Color3(0.2f, 0.9f, 1.0f); GL.Vertex3(0.0f, 1.0f, 0.0f);

            GL.End();
        }

        public override string Update(float dt)
        {
            return base.Update(dt);
        }

        public override void ProcessInputs(FrameEventArgs e)
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Key.Escape))
                ChangeState(StateTechDemo1.ID);

            if (state.IsKeyDown(Key.X))
                StateMan.Core.Exit();
        }

        #endregion Public Methods
    }
}