using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Constants;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Builders;
using OpenBreed.Rendering.Interface;

namespace OpenBreed.Gui.Elements
{
    internal class LabelField : Element, ILabelField
    {
        #region Internal Constructors

        internal LabelField(LabelFieldBuilder builder) : base(builder)
        {
            Text = builder.Text;
            Font = builder.GetFont();
            HorizontalAlignment = builder.HorizontalAlignment;
            VerticalAlignment = builder.VerticalAlignment;
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Text { get; set; }
        public IFont Font { get; }
        public HorizontalAlignment HorizontalAlignment { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }

        #endregion Public Properties
    }
}