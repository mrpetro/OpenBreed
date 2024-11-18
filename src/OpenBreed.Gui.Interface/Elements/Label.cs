using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Interface.Builders;

namespace OpenBreed.Gui.Interface.Elements
{
    internal class Label : Element, ILabel
    {
        #region Internal Constructors

        internal Label(LabelBuilder builder) : base(builder)
        {
            Text = builder.Text;
            HorizontalAlignment = builder.HorizontalAlignment;
            VerticalAlignment = builder.VerticalAlignment;
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Text { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }

    #endregion Public Properties
}
}