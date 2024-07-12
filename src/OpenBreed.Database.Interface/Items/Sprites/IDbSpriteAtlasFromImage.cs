using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenBreed.Database.Interface.Items.Sprites
{
    public interface IDbSpriteAtlasFromImage : IDbSpriteAtlas
    {
        #region Public Properties

        string DataRef { get; }

        ReadOnlyCollection<IDbSpriteCoords> Sprites { get; }

        #endregion Public Properties

        #region Public Methods

        void ClearCoords();
        void AddCoords(int x, int y, int width, int height);

        #endregion Public Methods
    }
}