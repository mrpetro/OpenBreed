using System.Collections.Generic;

namespace OpenBreed.Wecs.Abstractions.Primitives
{
    public interface IEntityTemplate
    {
        #region Public Properties

        string ClassName { get; }

        IEnumerable<IComponentTemplate> Components { get; }

        #endregion Public Properties
    }
}