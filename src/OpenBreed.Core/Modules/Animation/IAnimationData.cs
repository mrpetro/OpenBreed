namespace OpenBreed.Core.Modules.Animation
{
    public interface IAnimationData
    {
        #region Public Properties

        int Id { get; }
        string Name { get; }

        #endregion Public Properties
    }

    public interface IAnimationData<T> : IAnimationData
    {
        #region Public Properties

        float Length { get; }

        #endregion Public Properties

        #region Public Methods

        T GetFrame(float time);

        void AddFrame(T value, float frameTime);

        #endregion Public Methods
    }
}