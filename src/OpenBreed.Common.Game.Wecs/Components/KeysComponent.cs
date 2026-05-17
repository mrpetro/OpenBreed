using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Components
{
    public interface IKeysComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        int GeneralCount { get; }

        #endregion Public Properties
    }

    public class KeysComponent : IEntityComponent
    {
        #region Public Constructors

        public KeysComponent(
            int generalCount)
        {
            GeneralCount = generalCount;
        }

        #endregion Public Constructors

        #region Public Properties

        public int GeneralCount { get; set; }

        #endregion Public Properties
    }
}
