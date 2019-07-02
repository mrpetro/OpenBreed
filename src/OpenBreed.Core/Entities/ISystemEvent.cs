namespace OpenBreed.Core.Entities
{
    public interface ISystemEvent
    {
        #region Public Properties

        string Type { get; }
        object Data { get; }

        #endregion Public Properties
    }
}