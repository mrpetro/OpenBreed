using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Physics.Events
{
    public class BodyOnEvent : IEvent
    {
        #region Public Fields

        public const string TYPE = "BODY_ON";

        #endregion Public Fields

        #region Public Constructors

        public BodyOnEvent(IEntity entity)
        {
            Entity = entity;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }
        public string Type { get { return TYPE; } }

        #endregion Public Properties
    }
}
