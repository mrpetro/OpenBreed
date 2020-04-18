using OpenBreed.Core.Common.Systems.Components;
using System.Collections.Generic;

namespace OpenBreed.Core.Common.Components
{
    public class MachineState
    {
        #region Public Fields

        public int FsmId;
        public int StateId;

        #endregion Public Fields
    }

    public class FsmComponent : IEntityComponent
    {
        #region Public Constructors

        public FsmComponent()
        {
            States = new List<MachineState>();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<MachineState> States { get; }

        #endregion Public Properties
    }
}