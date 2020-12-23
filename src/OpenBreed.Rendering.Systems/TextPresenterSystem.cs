using OpenBreed.Core.Commands;
using OpenBreed.Core;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Systems;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Components;

namespace OpenBreed.Rendering.Systems
{
    public class TextPresenterSystem : WorldSystem, IRenderableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();

        #endregion Private Fields

        #region Internal Constructors

        public TextPresenterSystem(ICore core) : base(core)
        {
            Require<TextDataComponent>();
            Require<TextPresentationComponent>();
            Require<PositionComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

        public static void RegisterHandlers(ICommandsMan commands)
        {
        }

        public void Render(Box2 clipBox, int depth, float dt)
        {
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusConstantColor);
            GL.BlendColor(Color4.Black);
            //GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Texture2D);

            for (int i = 0; i < entities.Count; i++)
                RenderText(entities[i], clipBox);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private void RenderText(Entity entity, Box2 clipBox)
        {
            var pos = entity.Get<PositionComponent>();
            var tp = entity.Get<TextPresentationComponent>();
            var td = entity.Get<TextDataComponent>();

            GL.Enable(EnableCap.Texture2D);
            GL.PushMatrix();

            GL.Translate(pos.Value.X, pos.Value.Y, 0.0f);

            var caretPosX = 0.0f;
            var font = Core.GetModule<IRenderModule>().Fonts.GetById(tp.FontId);
            var height = font.Height;

            for (int i = 0; i < td.Data.Length; i++)
            {
                var ch = td.Data[i];

                switch (ch)
                {
                    case '\r':
                        GL.Translate(-caretPosX, 0.0f, 0.0f);
                        caretPosX = 0.0f;
                        continue;
                    case '\n':
                        GL.Translate(0.0f, -height, 0.0f);
                        continue;
                    default:
                        break;
                }

                font.Draw(ch);
                var width = font.GetWidth(ch);
                caretPosX += width;
                GL.Translate(width, 0.0f, 0.0f);
            }

            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

        #endregion Private Methods
    }
}