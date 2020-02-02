using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Rendering.Commands
{
    public class PutStampCommand : IWorldCommand
    {
        #region Public Fields

        public const string TYPE = "PUT_STAMP";

        #endregion Public Fields

        #region Public Constructors

        public PutStampCommand(int worldId, int stampId, int layerNo, Vector2 position)
        {
            WorldId = worldId;
            StampId = stampId;
            LayerNo = layerNo;
            Position = position;
        }

        #endregion Public Constructors

        #region Public Properties

        public int WorldId { get; }
        public int StampId { get; }

        /// <summary>
        /// TODO: Implement usage of this
        /// </summary>
        public int LayerNo { get; }
        public Vector2 Position { get; }

        public string Type { get { return TYPE; } }

        #endregion Public Properties
    }
}
