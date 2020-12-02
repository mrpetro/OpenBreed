using System.Collections.Generic;

namespace OpenBreed.Core.Components
{
    public class ShapeComponent : IEntityComponent
    {
        #region Public Constructors

        public ShapeComponent()
        {
            Items = new List<int>();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<int> Items { get; }

        #endregion Public Properties
    }
}