using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Drawing;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal class FontFromOSAtlas : IFont
    {
        #region Private Fields

        private readonly Dictionary<int, FontCharData> Lookup = new Dictionary<int, FontCharData>();
        private List<int> vboList = null;

        #endregion Private Fields

        #region Internal Constructors

        internal FontFromOSAtlas(FontFromOSAtlasGenerator builder)
        {
            Characters = builder.Characters;
            Id = builder.Id;
            MaximumCharacterSize = builder.MaximumCharacterSize;
            vboList = builder.vboList;
            Height = builder.Height;
            Lookup = builder.Lookup;
            Texture = builder.Texture;
        }

        #endregion Internal Constructors

        #region Public Properties

        public int[] Characters { get; }

        /// <summary>
        /// Id of this sprite atlas
        /// </summary>
        public int Id { get; }

        public Size MaximumCharacterSize { get; }

        public float Height { get; }

        #endregion Public Properties

        #region Internal Properties

        internal ITexture Texture { get; }

        #endregion Internal Properties

        #region Public Methods

        public float GetWidth(char character)
        {
            return Lookup[character].Width;
        }

        public float GetWidth(string text)
        {
            if (text.Length == 0)
            {
                return 0.0f;
            }
            var totalWidth = 0.0f;

            for (int i = 0; i < text.Length; i++)
            {
                totalWidth += Lookup[text[i]].Width;
            }

            return totalWidth;
        }

        public void Draw(IRenderView view, char character, Box2 clipBox, bool ignoreScale = false)
        {
            var found = Lookup[character];
            view.Context.Primitives.DrawSprite(
                view,
                Texture,
                vboList[found.Index],
                new Vector3(0, 0, 0),
                Vector2.One,
                Color4.White, ignoreScale);
        }

        public void Draw(IRenderView view, string text, Color4 color, Box2 clipBox, bool ignoreScale = false)
        {
            //TODO: include color in text rendering

            GL.BindTexture(TextureTarget.Texture2D, Texture.InternalId);

            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            //GL.BlendColor(125, 125, 125, 255);
            GL.Enable(EnableCap.Blend);

            var charPosX = 0.0f;

            var scaleCorrection = 1.0f;

            if (ignoreScale)
            {
                scaleCorrection = 1.0f / view.GetScale();
            }

            var height = Height * scaleCorrection;

            for (int i = 0; i < text.Length; i++)
            {
                var ch = text[i];
                var chData = Lookup[ch];

                var key = chData.Index;
                var width = chData.Width * scaleCorrection;
                var oX = chData.XOffset * scaleCorrection;

                //if (charPosX < clipBox.Min.X - width)
                //{
                //    charPosX += width;
                //    continue;
                //}

                //if (charPosX > clipBox.Max.X)
                //{
                //    break;
                //}

                var charPosition = new Vector3(charPosX + oX / 2.0f, 0.0f, 0.0f);

                view.Context.Primitives.DrawSprite(
                    view,
                    Texture,
                    vboList[key],
                    charPosition,
                    Vector2.One,
                    color, ignoreScale);

                var posT = new Vector2(width / 2.0f, height / 2.0f);
                posT += new Vector2(charPosition.X, charPosition.Y);

                //NOTE: Uncommect this to see character box
                //view.Context.Primitives.DrawRectangle(view, posT, new Vector2(width, height), Color4.Aqua, filled: false);
                charPosX += width;
            }

            GL.Disable(EnableCap.Blend);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Render(IRenderView view, string text, Box2 clipBox, Vector2 pos, float order)
        {
            GL.Enable(EnableCap.Texture2D);

            view.PushMatrix();
            view.Translate(new Vector3((int)pos.X, (int)pos.Y, 0.0f));

            var caretPosX = 0.0f;

            for (int i = 0; i < text.Length; i++)
            {
                var ch = text[i];

                switch (ch)
                {
                    case '\r':
                        view.Translate(new Vector3(-caretPosX, 0.0f, 0.0f));
                        caretPosX = 0.0f;
                        continue;
                    case '\n':
                        view.Translate(new Vector3(0.0f, -Height, 0.0f));
                        continue;
                    default:
                        break;
                }

                Draw(view, ch, clipBox);
                var width = GetWidth(ch);
                caretPosX += width;
                view.Translate(new Vector3(width, 0.0f, 0.0f));
            }

            view.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

        public void Load(IRenderContext renderContext)
        {
            vboList = new List<int>();

            int i = 0;

            foreach (var pair in Lookup)
            {
                var texCoord = new Vector2(i * MaximumCharacterSize.Width, 0);
                var vertices = CreateVertices(texCoord, pair.Value.Width - pair.Value.XOffset, Height);

                var vertexArrayBuilder = renderContext.Primitives.CreatePosTexCoordArray();
                vertexArrayBuilder.AddVertex(vertices[0].position, vertices[0].texCoord);
                vertexArrayBuilder.AddVertex(vertices[1].position, vertices[1].texCoord);
                vertexArrayBuilder.AddVertex(vertices[2].position, vertices[2].texCoord);
                vertexArrayBuilder.AddVertex(vertices[3].position, vertices[3].texCoord);

                vertexArrayBuilder.AddTriangleIndices(0, 1, 3);
                vertexArrayBuilder.AddTriangleIndices(1, 2, 3);

                var vao = vertexArrayBuilder.CreateTexturedVao();

                vboList.Add(vao);

                i++;
            }
        }

        #endregion Public Methods

        #region Internal Methods

        internal Vertex[] CreateVertices(Vector2 fontCoord, float fontWidth, float fontHeight)
        {
            var uvSize = new Vector2(fontWidth, fontHeight);
            uvSize = Vector2.Divide(uvSize, new Vector2(Texture.Width, Texture.Height));

            var uvLD = fontCoord;
            uvLD = Vector2.Divide(uvLD, new Vector2(Texture.Width, Texture.Height));
            var uvRT = Vector2.Add(uvLD, uvSize);

            Vertex[] vertices = {
                                new Vertex(new Vector2(0,   0),              new Vector2(uvLD.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(fontWidth,  0),        new Vector2(uvRT.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(fontWidth,  fontHeight), new Vector2(uvRT.X, uvLD.Y), Color4.White),
                                new Vertex(new Vector2(0,   fontHeight),       new Vector2(uvLD.X, uvLD.Y), Color4.White),
                            };

            return vertices;
        }

        #endregion Internal Methods
    }
}