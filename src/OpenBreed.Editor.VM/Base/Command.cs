using System;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Base
{
    public class Command : ICommand
    {
        #region Private Fields

        private Func<bool> _targetCanExecuteMethod;
        private Action _targetExecuteMethod;

        #endregion Private Fields

        #region Public Constructors

        public Command(Action executeMethod)
        {
            _targetExecuteMethod = executeMethod;
        }

        public Command(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _targetExecuteMethod = executeMethod;
            _targetCanExecuteMethod = canExecuteMethod;
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler CanExecuteChanged = delegate { };

        #endregion Public Events

        #region Public Methods

        bool ICommand.CanExecute(object parameter)
        {
            if (_targetCanExecuteMethod != null)
            {
                return _targetCanExecuteMethod();
            }

            if (_targetExecuteMethod != null)
            {
                return true;
            }

            return false;
        }

        void ICommand.Execute(object parameter)
        {
            if (_targetExecuteMethod != null)
            {
                _targetExecuteMethod();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        #endregion Public Methods
    }
}