﻿using OpenBreed.Model.Sprites;
using OpenBreed.Editor.VM.Base;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using OpenBreed.Common.Tools;

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
            Image = BitmapHelper.FromBytes(width, height, data);
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