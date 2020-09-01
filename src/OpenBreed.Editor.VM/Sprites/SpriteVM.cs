using OpenBreed.Common.Model.Sprites;
using OpenBreed.Editor.VM.Base;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteVM : BaseViewModel
    {
        #region Private Fields

        private Bitmap image;

        private int id;

        #endregion Private Fields

        #region Public Properties

        public Bitmap Image
        {
            get { return image; }
            set { SetProperty(ref image, value); }
        }

        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public static SpriteVM Create(SpriteModel spriteModel)
        {
            var newSprite = new SpriteVM();
            newSprite.FromModel(spriteModel);
            return newSprite;
        }

        public void UpdateBitmap(int width, int height, byte[] data)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            //Create a BitmapData and Lock all pixels to be written
            var bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height),
                                                    ImageLockMode.WriteOnly, bitmap.PixelFormat);

            //Copy the data from the byte array into BitmapData.Scan0
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);

            //Unlock the pixels
            bitmap.UnlockBits(bmpData);
            Image = bitmap;
        }

        public void FromModel(SpriteModel model)
        {
            UpdateBitmap(model.Width, model.Height, model.Data);
            Id = model.Index;
        }

        public void Draw(Graphics gfx, float x, float y, int factor)
        {
            int width = image.Width * factor;
            int height = image.Height * factor;

            gfx.DrawImage(image, (int)x, (int)y, width, height);
        }

        #endregion Public Methods
    }
}