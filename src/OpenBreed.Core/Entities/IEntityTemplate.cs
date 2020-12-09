using OpenBreed.Core.Components;
using System.Collections.Generic;

namespace OpenBreed.Core.Entities
{
    public interface IEntityTemplate
    {
        #region Public Properties

        IEnumerable<IComponentTemplate> Components { get; }

        #endregion Public Properties
    }
}