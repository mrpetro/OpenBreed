
using System.Collections.Generic;

namespace OpenBreed.Physics.Interface
{
    public interface IFixture
    {
        #region Public Properties

        List<int> GroupIds { get; }

        int Id { get; }

        IShape Shape { get; }

        #endregion Public Properties
    }
}