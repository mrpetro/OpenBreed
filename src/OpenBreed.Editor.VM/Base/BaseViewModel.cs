using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace OpenBreed.Editor.VM.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Protected Methods

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        protected bool SetProperty<TValue>(ref TValue storage, TValue value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<TValue>.Default.Equals(storage, value))
                return false;
            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        protected bool SetProperty<TTarget,TValue>(TTarget target, Expression<Func<TTarget, TValue>> properyExpression, TValue value, [CallerMemberName] string propertyName = "")
        {
            var expr = (MemberExpression)properyExpression.Body;
            var prop = (PropertyInfo)expr.Member;
            var oldValue = (TValue)prop.GetValue(target);

            if (EqualityComparer<TValue>.Default.Equals(oldValue, value))
            {
                return false;
            }

            prop.SetValue(target, value);

            this.OnPropertyChanged(propertyName);
            return true;
        }

        #endregion Protected Methods
    }
}