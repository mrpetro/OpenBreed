namespace OpenBreed.Database.Interface.Items.Animations
{
    public interface IDbAnimationFrame
    {
        #region Public Properties

        float Time { get; set; }

        #endregion Public Properties
    }

    public interface IDbAnimationFrame<TValue> : IDbAnimationFrame
    {
        #region Public Properties

        TValue Value { get; set; }

        #endregion Public Properties
    }
}