using System;

namespace OpenBreed.Core.Interface.Managers
{
    public interface IJobBuilder
    {
        IJobFinishBuilder DoAction(Action jobAction);
    }

    public interface IJobFinishBuilder
    {
        IJobBuilder OnFinish();
    }

    public interface IJobsMan
    {
        #region Public Methods

        void Execute(IJob job);

        void Update(float dt);
        IJobBuilder Create();

        #endregion Public Methods
    }
}