using System.Collections.Generic;

namespace OpenBreed.Core.Blueprints
{
    public interface IEntityDef
    {
        #region Public Properties

        string Name { get; }

        List<IComponentState> ComponentStates { get; }

        List<string> ComponentTypes { get; }

        #endregion Public Properties
    }
}