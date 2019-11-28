namespace OpenBreed.Core.Common.Helpers
{
    public interface IMsgHandler
    {
        #region Public Methods

        bool HandleMsg(object sender, IMsg msg);

        #endregion Public Methods
    }
}