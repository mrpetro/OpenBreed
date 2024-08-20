using OpenTK.Windowing.Common;
using System;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Base
{
    public class DelegateCommand<T> : ICommand
    {
        #region Private Fields

        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        #endregion Private Fields

        #region Public Constructors

        public DelegateCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public DelegateCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler CanExecuteChanged;

        #endregion Public Events

        #region Public Methods

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            return _canExecute((parameter == null) ? default(T) : (T)Convert.ChangeType(parameter, typeof(T)));
        }

        public void Execute(object parameter)
        {
            _execute((parameter == null) ? default(T) : (T)Convert.ChangeType(parameter, typeof(T)));
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        #endregion Public Methods
    }

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