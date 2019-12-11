using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;
using OpenBreed.Core.States;
using System.Linq;

namespace OpenBreed.Core.Common.Helpers
{
    public class StateChangeCommandHandler : IMsgHandler
    {
        #region Private Fields

        private World world;

        #endregion Private Fields

        #region Public Constructors

        public StateChangeCommandHandler(World world)
        {
            this.world = world;
        }

        #endregion Public Constructors

        #region Public Methods

        public bool Handle(object sender, IMsg cmd)
        {
            return HandleStateChangeCommand(sender, (StateChangeCommand)cmd);
        }

        #endregion Public Methods

        #region Private Methods

        private bool HandleStateChangeCommand(object sender, StateChangeCommand cmd)
        {
            var entity = world.Core.Entities.GetById(cmd.EntityId);

            var fsm = entity.FsmList.FirstOrDefault(item => item.Name == cmd.FsmName);

            if(fsm != null)
                fsm.Handle(sender, cmd);

            return true;
        }

        public bool EnqueueMsg(object sender, IMsg msg)
        {
            return false;
        }

        #endregion Private Methods
    }
}