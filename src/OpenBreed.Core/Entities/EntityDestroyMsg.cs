using OpenBreed.Core.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Entities
{
    public class EntityDestroyMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "ENTITY_DESTROY";

        #endregion Public Fields

        #region Public Constructors

        public EntityDestroyMsg(IEntity entity)
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
