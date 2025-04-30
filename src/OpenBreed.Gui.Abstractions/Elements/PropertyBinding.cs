using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace OpenBreed.Gui.Abstractions.Elements
{
    public interface IValueBinding<TValue>
    {
        #region Public Methods

        TValue? GetValue();

        void SetValue(TValue newValue);

        #endregion Public Methods
    }

    public class ValueBinding<TValue> : IValueBinding<TValue>
    {
        #region Private Fields

        private TValue value;

        #endregion Private Fields

        #region Private Constructors

        private ValueBinding(TValue value)
        {
            this.value = value;
        }

        #endregion Private Constructors

        #region Public Methods

        public static ValueBinding<TValue> Create(TValue value)
        {
            return new ValueBinding<TValue>(value);
        }

        public TValue? GetValue() => value;

        public void SetValue(TValue newValue)
        {
            this.value = newValue;
        }

        #endregion Public Methods
    }

    public class PropertyBinding<TValue> : IValueBinding<TValue>
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

        public void SetValue(TValue newValue)
        {
            propertyInfo.SetValue(propertyTarget, newValue);
        }

        #endregion Public Methods
    }
}