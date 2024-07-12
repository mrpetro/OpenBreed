using OpenBreed.Wecs.Components;
using System.Collections.Generic;
using System.Data;
using OpenTK;
using OpenTK.Mathematics;
using OpenBreed.Physics.Interface.Managers;

namespace OpenBreed.Wecs.Components.Physics
{
    public class ResponseComponent : IEntityComponent
    {
        #region Public Constructors

        public ResponseComponent()
        {
            Contacts = new List<CollisionContact>();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<CollisionContact> Contacts { get; }

        #endregion Public Properties
    }
}