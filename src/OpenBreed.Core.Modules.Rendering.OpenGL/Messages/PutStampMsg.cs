using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Rendering.Messages
{
    public class PutStampMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "PUT_STAMP";

        #endregion Public Fields

        #region Public Constructors

        public PutStampMsg(IEntity entity, int stampId, int layerNo, Vector2 position)
        {
            Entity = entity;
            StampId = stampId;
            LayerNo = layerNo;
            Position = position;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }
        public int StampId { get; }
        public int LayerNo { get; }
        public Vector2 Position { get; }

        public string Type { get { return TYPE; } }

        #endregion Public Properties
    }
}
