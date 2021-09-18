using OpenBreed.Core.Commands;
using OpenBreed.Core;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Managers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Wecs.Systems.Rendering
{
    public class TextPresenterSystem : SystemBase, IRenderableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
        private readonly IFontMan fontMan;

        #endregion Private Fields

        #region Internal Constructors

        internal TextPresenterSystem(IFontMan fontMan)
        {
            this.fontMan = fontMan;

            RequireEntityWith<TextDataComponent>();
            RequireEntityWith<TextPresentationComponent>();
            RequireEntityWith<PositionComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

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

            GL.Translate((int)pos.Value.X, (int)pos.Value.Y, 0.0f);

            var caretPosX = 0.0f;
            var font = fontMan.GetById(tp.FontId);
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