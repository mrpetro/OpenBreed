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

    public sealed class KeysComponentFactory : ComponentFactoryBase<IKeysComponentTemplate>
    {
        #region Public Constructors

        public KeysComponentFactory()
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IKeysComponentTemplate template)
        {
            return new KeysComponent(
                template.GeneralCount);
        }

        #endregion Protected Methods
    }
}
