using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Model.Sprites;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteFromImageVM : SpriteVM
    {
        #region Private Fields

        private MyRectangle _sourceRectangle;

        #endregion Private Fields

        #region Public Constructors

        public SpriteFromImageVM(IBitmapProvider bitmapProvider) : base(bitmapProvider)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public MyRectangle SourceRectangle
        {
            get { return _sourceRectangle; }
            set { SetProperty(ref _sourceRectangle, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public static SpriteFromImageVM Create(IBitmapProvider bitmapProvider, SpriteModel spriteModel, MyRectangle sourceRectangle)
        {
            var vm = new SpriteFromImageVM(bitmapProvider);
            vm.FromModel(spriteModel);
            vm.SourceRectangle = sourceRectangle;
            return vm;
        }

        #endregion Public Methods
    }
}