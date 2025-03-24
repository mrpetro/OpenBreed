using OpenBreed.Gui.Abstractions.Builders;
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
    internal class StateboxBuilder : ElementBuilder<IStatebox>, IStateboxBuilder
    {
        #region Public Constructors

        public StateboxBuilder()
        {
        }

        #endregion Public Constructors

        #region Internal Properties

        internal bool IsChecked { get; private set; }

        internal PropertyBinding<bool>? IsCheckedBinding { get; private set; }


        internal (object propertyTarget, PropertyInfo properyExpression) StateBinding { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public void BindIsChecked(PropertyBinding<bool>? binding)
        {
            IsCheckedBinding = binding;
        }

        public void BindIsChecked<TTarget>(TTarget target, Expression<Func<TTarget, bool>> properyExpression)
        {
            var expr = (MemberExpression)properyExpression.Body;
            var prop = (PropertyInfo)expr.Member;

            IsCheckedBinding = new PropertyBinding<bool>(target, prop);
        }

        public void SetChecked(bool isChecked)
        {
            IsChecked = isChecked;
        }

        #endregion Public Methods

        #region Internal Methods

        public override IStatebox Build()
        {
            return new Statebox(this);
        }

        #endregion Internal Methods
    }
}