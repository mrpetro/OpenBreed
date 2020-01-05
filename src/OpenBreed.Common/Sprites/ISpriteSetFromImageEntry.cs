using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenBreed.Common.Sprites
{
    public interface ISpriteSetFromImageEntry : ISpriteSetEntry
    {
        #region Public Properties

        string DataRef { get; }

        ReadOnlyCollection<ISpriteCoords> Sprites { get; }

        #endregion Public Properties

        #region Public Methods

        void ClearCoords();
        void AddCoords(int x, int y, int width, int height);

        #endregion Public Methods
    }
}