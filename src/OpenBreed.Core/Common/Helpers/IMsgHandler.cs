namespace OpenBreed.Core.Common.Helpers
{
    public interface IMsgHandler
    {
        #region Public Methods

        bool RecieveMsg(object sender, IMsg msg);

        bool EnqueueMsg(object sender, IEntityMsg msg);

        #endregion Public Methods
    }
}