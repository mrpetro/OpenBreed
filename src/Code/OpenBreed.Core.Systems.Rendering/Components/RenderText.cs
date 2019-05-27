//using OpenBreed.Core.Entities;
//using OpenBreed.Core.Systems.Common.Components;
//using OpenBreed.Core.Systems.Rendering.Helpers;
//using OpenTK;
//using OpenTK.Graphics;
//using OpenTK.Graphics.OpenGL;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;

//namespace OpenBreed.Core.Systems.Rendering.Components
//{
//    public class RenderText : IRenderComponent
//    {
//        #region Public Fields

//        public const string Characters = @"qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789µ§½!""#¤%&/()=?^*@£€${[]}\~¨'-_.:,;<>|°©®±¥";
//        public static readonly float CharacterWidthNormalized;

//        // 21x48 per char,
//        public readonly List<RenderCharacter> Text;

//        #endregion Public Fields

//        #region Private Fields

//        private static readonly Dictionary<char, int> Lookup;
//        private readonly Color4 color;
//        private Position position;

//        #endregion Private Fields

//        #region Public Constructors

//        static RenderText()
//        {
//            Lookup = new Dictionary<char, int>();
//            for (int i = 0; i < Characters.Length; i++)
//            {
//                if (!Lookup.ContainsKey(Characters[i]))
//                    Lookup.Add(Characters[i], i);
//            }
//            CharacterWidthNormalized = 1f / Characters.Length;
//        }

//        public RenderText(Color4 color, string value)
//        {
//            this.color = color;
//            Text = new List<RenderCharacter>(value.Length);
//            SetText(value);
//        }

//        #endregion Public Constructors

//        #region Public Properties

//        public Type SystemType { get { return typeof(RenderSystem); } }

//        #endregion Public Properties

//        #region Public Methods

//        public void SetText(string value)
//        {
//            var startPos = position.Current;

//            Text.Clear();
//            for (int i = 0; i < value.Length; i++)
//            {
//                int offset;
//                if (Lookup.TryGetValue(value[i], out offset))
//                {
//                    var c = new RenderCharacter(new Vector2((i * 0.015f), 0.0f),
//                        (offset * CharacterWidthNormalized)); ;
//                    Text.Add(c);
//                }
//            }
//        }

//        /// <summary>
//        /// Draw this tile to given viewport
//        /// </summary>
//        /// <param name="viewport">Viewport which this tile will be rendered to</param>
//        public void Draw(Viewport viewport)
//        {
//            GL.PushMatrix();

//            GL.Translate(position.Current.X, position.Current.Y, 0.0f);

//            for (int i = 0; i < Text.Count; i++)
//            {
//                var c = Text[i];
//                c.Draw(viewport);
//            }

//            GL.PopMatrix();
//        }

//        public void Initialize(IEntity entity)
//        {
//            position = entity.Components.OfType<Position>().First();
//        }

//        public void Deinitialize(IEntity entity)
//        {
//            throw new NotImplementedException();
//        }

//        public Bitmap GenerateCharacters(int fontSize, string fontName, out Size charSize)
//        {
//            var characters = new List<Bitmap>();
//            using (var font = new Font(fontName, fontSize))
//            {
//                for (int i = 0; i < Characters.Length; i++)
//                {
//                    var charBmp = GenerateCharacter(font, Characters[i]);
//                    characters.Add(charBmp);
//                }
//                charSize = new Size(characters.Max(x => x.Width), characters.Max(x => x.Height));
//                var charMap = new Bitmap(charSize.Width * characters.Count, charSize.Height);
//                using (var gfx = Graphics.FromImage(charMap))
//                {
//                    gfx.FillRectangle(Brushes.Black, 0, 0, charMap.Width, charMap.Height);
//                    for (int i = 0; i < characters.Count; i++)
//                    {
//                        var c = characters[i];
//                        gfx.DrawImageUnscaled(c, i * charSize.Width, 0);

//                        c.Dispose();
//                    }
//                }
//                return charMap;
//            }
//        }

//        #endregion Public Methods

//        #region Private Methods

//        private Bitmap GenerateCharacter(Font font, char c)
//        {
//            var size = GetSize(font, c);
//            var bmp = new Bitmap((int)size.Width, (int)size.Height);
//            using (var gfx = Graphics.FromImage(bmp))
//            {
//                gfx.FillRectangle(Brushes.Black, 0, 0, bmp.Width, bmp.Height);
//                gfx.DrawString(c.ToString(), font, Brushes.White, 0, 0);
//            }
//            return bmp;
//        }

//        private SizeF GetSize(Font font, char c)
//        {
//            using (var bmp = new Bitmap(512, 512))
//            {
//                using (var gfx = Graphics.FromImage(bmp))
//                {
//                    return gfx.MeasureString(c.ToString(), font);
//                }
//            }
//        }

//        #endregion Private Methods
//    }
//}