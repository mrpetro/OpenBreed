using System.Linq.Expressions;
using System.Reflection;

namespace OpenBreed.Gui.Abstractions.Elements
{
    public class PropertyBinding<TValue>
    {
        #region Private Fields

        private object propertyTarget;
        private PropertyInfo propertyInfo;

        #endregion Private Fields

        #region Public Constructors

        public PropertyBinding(object propertyTarget, PropertyInfo propertyInfo)
        {
            this.propertyTarget = propertyTarget ?? throw new ArgumentNullException(nameof(propertyTarget));
            this.propertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        }

        #endregion Public Constructors

        #region Public Methods

        public static PropertyBinding<TValue> Create<TTarget>(TTarget target, Expression<Func<TTarget, TValue>> properyExpression)
        {
            var expr = (MemberExpression)properyExpression.Body;
            var prop = (PropertyInfo)expr.Member;

            return new PropertyBinding<TValue>(target, prop);
        }

        public TValue? GetValue()
        {
            var value = propertyInfo.GetValue(propertyTarget);

            if (value is null)
            {
                return default;
            }

            return (TValue)value;
        }

        public void SetValue(bool isChecked)
        {
            propertyInfo.SetValue(propertyTarget, isChecked);
        }

        #endregion Public Methods
    }
}