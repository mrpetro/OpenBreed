using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Interface.Elements;

namespace OpenBreed.Gui.Interface.Builders
{
    internal class InteractiveLabelBuilder : InteractiveElementBuilder, IInteractiveLabelBuilder
    {
        #region Internal Fields

        internal string Text;
        internal HorizontalAlignment HorizontalAlignment;
        internal VerticalAlignment VerticalAlignment;

        #endregion Internal Fields

        #region Public Constructors

        public InteractiveLabelBuilder(InteractionCore interactionCore, InteractiveElementBuilder parentBuilder)
                    : base(interactionCore, parentBuilder)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void SetText(string text)
        {
            Text = text;
        }

        public void SetHorizontalAlignment(HorizontalAlignment horizontalAlignment)
        {
            HorizontalAlignment = horizontalAlignment;
        }

        public void SetVerticalAlignment(VerticalAlignment verticalAlignment)
        {
            VerticalAlignment = verticalAlignment;
        }

        #endregion Public Methods

        #region Internal Methods

        internal override InteractiveElement InternalBuild()
        {
            return new InteractiveLabel(this);
        }

        #endregion Internal Methods
    }
}