using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Sandbox.App.Components
{
    public class PathfindRequestComponent : IEntityComponent
    {
        #region Public Constructors

        public PathfindRequestComponent(int requestId)
        {
            RequestId = requestId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int RequestId { get; }

        #endregion Public Properties
    }
}