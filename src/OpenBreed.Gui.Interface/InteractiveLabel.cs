using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Interface.Builders;

namespace OpenBreed.Gui.Interface
{
    internal class InteractiveLabel : InteractiveElement, IInteractiveLabel
    {
        #region Internal Constructors

        public string Text { get; set; }

        internal InteractiveLabel(InteractiveLabelBuilder builder) : base(builder)
        {
            Text = builder.Text;
        }

        #endregion Internal Constructors

        #region Public Properties

        #endregion Public Properties
    }
}