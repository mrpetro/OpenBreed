namespace OpenBreed.Database.Interface.Items.Animations
{
    public interface IAnimationFrame
    {
        #region Public Properties

        float Time { get; set; }

        #endregion Public Properties
    }

    public interface IAnimationFrame<TValue> : IAnimationFrame
    {
        #region Public Properties

        TValue Value { get; set; }

        #endregion Public Properties
    }
}