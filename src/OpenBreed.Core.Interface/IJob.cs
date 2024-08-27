using System;

namespace OpenBreed.Core.Interface
{
    /// <summary>
    /// Interface for Job object
    /// </summary>
    public interface IJob : IDisposable
    {
        #region Public Properties

        /// <summary>
        /// Action which should be called when job is completed
        /// </summary>
        Action<IJob> Complete { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Starts executing this job
        /// </summary>
        void Execute();

        /// <summary>
        /// Updates job state using given time step
        /// </summary>
        /// <param name="dt">Time step</param>
        void Update(float dt);

        #endregion Public Methods
    }
}