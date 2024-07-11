using OpenBreed.Wecs.Components;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Entities
{
    public interface IEntityTemplate
    {
        #region Public Properties

        IEnumerable<IComponentTemplate> Components { get; }

        #endregion Public Properties
    }
}