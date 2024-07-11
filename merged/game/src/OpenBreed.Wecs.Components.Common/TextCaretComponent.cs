using OpenBreed.Wecs.Components;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Components.Common
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