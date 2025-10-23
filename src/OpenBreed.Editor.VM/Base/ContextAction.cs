using System;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Base
{
    public class ContextAction : BaseViewModel
    {
        #region Public Constructors

        public ContextAction(string headerText, Action action, Func<bool> canExecute = null)
        {
            HeaderText = headerText;
            Command = new Command(action, canExecute);
        }

        #endregion Public Constructors

        #region Public Properties

        public string HeaderText { get; }
        public ICommand Command { get; }

        #endregion Public Properties
    }
}