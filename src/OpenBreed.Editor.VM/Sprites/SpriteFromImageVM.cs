using OpenBreed.Model.Sprites;
using System.Drawing;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteFromImageVM : SpriteVM
    {
        #region Private Fields

        private Rectangle _sourceRectangle;

        #endregion Private Fields

        #region Public Properties

        public Rectangle SourceRectangle
        {
            get { return _sourceRectangle; }
            set { SetProperty(ref _sourceRectangle, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public static SpriteFromImageVM Create(SpriteModel spriteModel, Rectangle sourceRectangle)
        {
            var vm = new SpriteFromImageVM();
            vm.FromModel(spriteModel);
            vm.SourceRectangle = sourceRectangle;
            return vm;
        }

        #endregion Public Methods
    }
}