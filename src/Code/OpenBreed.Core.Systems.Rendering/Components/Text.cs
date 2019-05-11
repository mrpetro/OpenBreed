using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Rendering.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Core.Systems.Rendering.Components
{
    public class Text : IRenderComponent
    {
        #region Private Fields

        private const string Characters = @"qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789µ§½!""#¤%&/()=?^*@£€${[]}\~¨'-_.:,;<>|°©®±¥";

        #endregion Private Fields

        #region Public Properties

        public Type SystemType => throw new NotImplementedException();

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Draw(Viewport viewport)
        {
            throw new NotImplementedException();
        }

        public Bitmap GenerateCharacters(int fontSize, string fontName, out Size charSize)
        {
            var characters = new List<Bitmap>();
            using (var font = new Font(fontName, fontSize))
            {
                for (int i = 0; i < Characters.Length; i++)
                {
                    var charBmp = GenerateCharacter(font, Characters[i]);
                    characters.Add(charBmp);
                }
                charSize = new Size(characters.Max(x => x.Width), characters.Max(x => x.Height));
                var charMap = new Bitmap(charSize.Width * characters.Count, charSize.Height);
                using (var gfx = Graphics.FromImage(charMap))
                {
                    gfx.FillRectangle(Brushes.Black, 0, 0, charMap.Width, charMap.Height);
                    for (int i = 0; i < characters.Count; i++)
                    {
                        var c = characters[i];
                        gfx.DrawImageUnscaled(c, i * charSize.Width, 0);

                        c.Dispose();
                    }
                }
                return charMap;
            }
        }

        public void Initialize(IEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Private Methods

        private Bitmap GenerateCharacter(Font font, char c)
        {
            var size = GetSize(font, c);
            var bmp = new Bitmap((int)size.Width, (int)size.Height);
            using (var gfx = Graphics.FromImage(bmp))
            {
                gfx.FillRectangle(Brushes.Black, 0, 0, bmp.Width, bmp.Height);
                gfx.DrawString(c.ToString(), font, Brushes.White, 0, 0);
            }
            return bmp;
        }

        private SizeF GetSize(Font font, char c)
        {
            using (var bmp = new Bitmap(512, 512))
            {
                using (var gfx = Graphics.FromImage(bmp))
                {
                    return gfx.MeasureString(c.ToString(), font);
                }
            }
        }

        #endregion Private Methods
    }
}