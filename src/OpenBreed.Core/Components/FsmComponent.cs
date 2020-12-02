using OpenBreed.Core.Builders;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Components
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

        public FsmComponent(FsmComponentBuilder builder)
        {
            States = builder.States.ToList();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<MachineState> States { get; }

        #endregion Public Properties
    }
}