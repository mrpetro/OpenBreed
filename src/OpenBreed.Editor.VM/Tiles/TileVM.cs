using System;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Model.Tiles;

namespace OpenBreed.Editor.VM.Tiles
{
    public class TileVM
    {

        #region Public Constructors

        public TileVM(int index, MyRectangle rectangle)
        {
            Index = index;
            Rectangle = rectangle;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Index { get; private set; }
        public MyRectangle Rectangle { get; private set; }

        internal static TileVM Create(TileModel tile)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties

    }
}