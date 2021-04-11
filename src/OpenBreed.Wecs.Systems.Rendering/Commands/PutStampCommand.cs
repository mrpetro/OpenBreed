using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Commands;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Rendering.Commands
{
    public class PutStampCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "PUT_STAMP";

        #endregion Public Fields

        #region Public Constructors

        public PutStampCommand(int entityId, int stampId, int layerNo, Vector2 position)
        {
            EntityId = entityId;
            StampId = stampId;
            LayerNo = layerNo;
            Position = position;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public int StampId { get; }

        /// <summary>
        /// TODO: Implement usage of this
        /// </summary>
        public int LayerNo { get; }
        public Vector2 Position { get; }

        public string Name { get { return TYPE; } }

        #endregion Public Properties
    }
}
