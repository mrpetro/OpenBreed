namespace OpenBreed.Database.Interface.Items.Animations
{
    public interface IDbAnimationFrame
    {
        #region Public Properties

        float Time { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Copy this object
        /// </summary>
        /// <returns>Copy of this object</returns>
        IDbAnimationFrame Copy();

        #endregion Public Methods
    }

    public interface IDbAnimationFrame<TValue> : IDbAnimationFrame
    {
        #region Public Properties

        TValue Value { get; set; }

        #endregion Public Properties
    }
}