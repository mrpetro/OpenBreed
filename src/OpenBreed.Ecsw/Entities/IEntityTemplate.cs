using OpenBreed.Ecsw.Components;
using System.Collections.Generic;

namespace OpenBreed.Ecsw.Entities
{
    public interface IEntityTemplate
    {
        #region Public Properties

        IEnumerable<IComponentTemplate> Components { get; }

        #endregion Public Properties
    }
}