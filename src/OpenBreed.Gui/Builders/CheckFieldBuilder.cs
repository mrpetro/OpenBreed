using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Controllers;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Elements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Builders
{
    internal class CheckFieldBuilder : ElementBuilder<ICheckField>, ICheckFieldBuilder
    {
        #region Public Constructors

        public CheckFieldBuilder(IElementInputController<ICheckField> inputController) : base(inputController)
        {

            SetSize(25, 25);
        }

        #endregion Public Constructors

        #region Internal Properties

        internal bool Value { get; private set; }

        internal PropertyBinding<bool>? ValueBinding { get; private set; }


        internal (object propertyTarget, PropertyInfo properyExpression) StateBinding { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public void BindProperty(PropertyBinding<bool>? binding)
        {
            ValueBinding = binding;
        }

        public void BindIsChecked<TTarget>(TTarget target, Expression<Func<TTarget, bool>> properyExpression)
        {
            var expr = (MemberExpression)properyExpression.Body;
            var prop = (PropertyInfo)expr.Member;

            ValueBinding = new PropertyBinding<bool>(target, prop);
        }

        public void SetChecked(bool isChecked)
        {
            Value = isChecked;
        }

        #endregion Public Methods

        #region Internal Methods

        public override Abstractions.Elements.ICheckField Build()
        {
            return new Elements.CheckField(this);
        }

        #endregion Internal Methods
    }
}