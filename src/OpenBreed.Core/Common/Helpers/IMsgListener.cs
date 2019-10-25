namespace OpenBreed.Core.Common.Helpers
{
    public interface IMsgListener
    {
        #region Public Methods

        bool RecieveMsg(object sender, IMsg msg);

        #endregion Public Methods
    }
}