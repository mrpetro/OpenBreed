namespace OpenBreed.Core.Interface.Managers
{
    public interface IMsg
    {
    }

    public interface IMessagesMan
    {
        #region Public Methods

        void RemoveMsgData(int msgId);

        IMsg GetMsgData(int msgId);

        int SetMsgData(IMsg data);

        #endregion Public Methods
    }
}