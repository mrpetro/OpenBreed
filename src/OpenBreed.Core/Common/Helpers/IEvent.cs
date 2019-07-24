namespace OpenBreed.Core.Common.Helpers
{
    public interface IEvent
    {
        #region Public Properties

        string Type { get; }
        object Data { get; }

        #endregion Public Properties
    }
}