using OpenBreed.Gui.Interface.Elements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Builders
{
    internal class CheckboxBuilder : ElementBuilder, ICheckboxBuilder
    {
        #region Public Constructors

        public CheckboxBuilder(ElementBuilder parentBuilder)
                    : base(parentBuilder)
        {
        }

        #endregion Public Constructors

        #region Internal Properties

        internal bool IsChecked { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public void SetChecked(bool isChecked)
        {
            IsChecked = isChecked;
        }

        #endregion Public Methods

        #region Internal Methods

        internal override Element InternalBuild()
        {
            return new Checkbox(this);
        }

        #endregion Internal Methods
    }
}