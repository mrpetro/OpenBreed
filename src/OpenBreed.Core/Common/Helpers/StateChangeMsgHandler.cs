using OpenBreed.Core.Entities;
using OpenBreed.Core.States;
using System.Linq;

namespace OpenBreed.Core.Common.Helpers
{
    public class StateChangeMsgHandler : IMsgHandler
    {
        #region Private Fields

        private World world;

        #endregion Private Fields

        #region Public Constructors

        public StateChangeMsgHandler(World world)
        {
            this.world = world;
        }

        #endregion Public Constructors

        #region Public Methods

        public bool HandleMsg(object sender, IMsg msg)
        {
            return HandleEntityMsg(sender, (StateChangeMsg)msg);
        }

        #endregion Public Methods

        #region Private Methods

        private bool HandleEntityMsg(object sender, StateChangeMsg msg)
        {
            var entity = world.Core.Entities.GetById(msg.EntityId);

            var fsm = entity.FsmList.FirstOrDefault(item => item.Name == msg.FsmName);

            if(fsm != null)
                fsm.HandleMsg(sender, msg);

            return true;
        }

        public bool EnqueueMsg(object sender, IMsg msg)
        {
            return false;
        }

        #endregion Private Methods
    }
}