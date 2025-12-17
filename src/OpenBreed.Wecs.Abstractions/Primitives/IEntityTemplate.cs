using System.Collections.Generic;

namespace OpenBreed.Wecs.Abstractions.Primitives
{
    public interface IEntityTemplate
    {
        #region Public Properties

        IEnumerable<IComponentTemplate> Components { get; }

        #endregion Public Properties
    }
}