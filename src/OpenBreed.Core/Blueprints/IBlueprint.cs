using System.Collections.Generic;

namespace OpenBreed.Core.Blueprints
{
    public interface IBlueprint
    {
        #region Public Properties

        string Name { get; }

        List<IEntityDef> Entities { get; }

        #endregion Public Properties
    }
}