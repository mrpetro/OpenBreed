using OpenBreed.Core.Entities;

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
            return HandleEntityMsg(sender, (IEntityMsg)msg);
        }

        #endregion Public Methods

        #region Private Methods

        private bool HandleEntityMsg(object sender, IEntityMsg msg)
        {
            msg.Entity.StateMachine.HandleMsg(sender, msg);

            return true;
        }

        #endregion Private Methods
    }
}