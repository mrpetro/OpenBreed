namespace OpenBreed.Core.Blueprints
{
    public interface IComponentState
    {
        #region Public Properties

        string Key { get; }

        object Value { get; }

        #endregion Public Properties
    }
}