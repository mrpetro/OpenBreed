using System;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Model.Tiles;

namespace OpenBreed.Editor.VM.Tiles
{
    public class TileVM
    {
        #region Public Constructors

        public TileVM(int index, MyPoint indexPosition)
        {
            Index = index;
            IndexPosition = indexPosition;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Index { get; }
        public MyPoint IndexPosition { get; }

        #endregion Public Properties

        #region Public Methods

        public MyRectangle GetBox(int tileSize)
        {
            return new MyRectangle(IndexPosition.X * tileSize, IndexPosition.Y * tileSize, tileSize, tileSize);
        }

        #endregion Public Methods

        #region Internal Methods

        internal static TileVM Create(TileModel tile)
        {
            throw new NotImplementedException();
        }

        #endregion Internal Methods
    }
}