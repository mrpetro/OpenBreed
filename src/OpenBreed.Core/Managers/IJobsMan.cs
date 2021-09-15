namespace OpenBreed.Core.Managers
{
    public interface IJobsMan
    {
        #region Public Methods

        void Execute(IJob job);

        void Update(float dt);

        #endregion Public Methods
    }
}