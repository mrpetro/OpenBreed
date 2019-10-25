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

        public bool RecieveMsg(object sender, IMsg msg)
        {
            return HandleEntityMsg(sender, (StateChangeMsg)msg);
        }

        #endregion Public Methods

        #region Private Methods

        private bool HandleEntityMsg(object sender, StateChangeMsg msg)
        {
            var fsm = msg.Entity.FsmList.FirstOrDefault(item => item.Name == msg.FsmName);

            if(fsm != null)
                fsm.RecieveMsg(sender, msg);

            return true;
        }

        public bool EnqueueMsg(object sender, IEntityMsg msg)
        {
            return false;
        }

        #endregion Private Methods
    }
}