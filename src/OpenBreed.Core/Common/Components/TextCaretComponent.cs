using OpenBreed.Core.Common.Systems.Components;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace OpenBreed.Core.Common.Components
{
    public class TextCaretComponent : IEntityComponent
    {
        #region Internal Constructors

        public TextCaretComponent()
        {

        }

        #endregion Internal Constructors

        #region Public Properties

        public int Position { get; set; }

        #endregion Public Properties
    }
}