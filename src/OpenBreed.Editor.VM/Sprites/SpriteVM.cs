using OpenBreed.Common.Sprites;
using OpenBreed.Editor.VM.Base;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteVM : BaseViewModel
    {

        #region Private Fields

        private Bitmap _bitmap;

        private int _id;

        #endregion Private Fields

        #region Public Properties

        public Bitmap Bitmap
        {
            get { return _bitmap; }
            set { SetProperty(ref _bitmap, value); }
        }
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public static SpriteVM Create(SpriteModel spriteModel)
        {
            var bitmap = new Bitmap(spriteModel.Width, spriteModel.Height, PixelFormat.Format8bppIndexed);

            //Create a BitmapData and Lock all pixels to be written
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, spriteModel.Width, spriteModel.Height),
                                                    ImageLockMode.WriteOnly, bitmap.PixelFormat);

            //Copy the data from the byte array into BitmapData.Scan0
            Marshal.Copy(spriteModel.Data, 0, bmpData.Scan0, spriteModel.Data.Length);

            //Unlock the pixels
            bitmap.UnlockBits(bmpData);

            var newSprite = new SpriteVM();
            newSprite.ID = spriteModel.Index;
            newSprite.Bitmap = bitmap;
            return newSprite;
        }

        public void Draw(Graphics gfx, float x, float y, int factor)
        {
            int width = _bitmap.Width * factor;
            int height = _bitmap.Height * factor;

            gfx.DrawImage(_bitmap, (int)x, (int)y, width, height);
        }

        #endregion Public Methods

    }
}