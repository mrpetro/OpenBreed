using OpenTK;
using System.Drawing;

namespace OpenBreed.Core
{
    public interface ICoreClient
    {
        #region Public Properties

        /// <summary>
        /// Client display transformation matrix
        /// </summary>
        Matrix4 ClientTransform { get; }

        /// <summary>
        /// Client display aspect ratio
        /// </summary>
        float ClientRatio { get; }

        /// <summary>
        /// Client display rectangle
        /// </summary>
        Rectangle ClientRectangle { get; }

        /// <summary>
        /// Exits the application
        /// TODO: Show not be part of this inteface
        /// </summary>
        void Exit();

        /// <summary>
        /// Start running main aplication loop
        /// TODO: Show not be part of this inteface
        /// </summary>
        void Run();

        #endregion Public Properties
    }
}