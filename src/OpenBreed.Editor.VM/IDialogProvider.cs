using System.Linq;

namespace OpenBreed.Editor.VM
{
    public enum DialogAnswer
    {
        None = 0,
        OK = 1,
        Cancel = 2,
        Abort = 3,
        Retry = 4,
        Ignore = 5,
        Yes = 6,
        No = 7
    }

    public enum QuestionDialogButtons
    {
        OK = 0,
        OKCancel = 1,
        AbortRetryIgnore = 2,
        YesNoCancel = 3,
        YesNo = 4,
        RetryCancel = 5
    }

    public interface IDialogProvider
    {
        #region Public Methods

        void ShowMessage(string text, string caption);

        IFolderBrowserQuery FolderBrowserDialog();
        IOpenFileQuery OpenFileDialog();
        ISaveFileQuery SaveFileDialog();

        DialogAnswer ShowReplaceFileQuestion(string title, string caption);

        #endregion Public Methods
    }

    public class FolderBrowserResult
    {
        #region Public Constructors

        public FolderBrowserResult(DialogAnswer answer, string selectedDirectory)
        {
            Answer = answer;
            SelectedDirectory = selectedDirectory;
        }

        #endregion Public Constructors

        #region Public Properties

        public DialogAnswer Answer { get; private set; }
        public string SelectedDirectory { get; private set; }

        #endregion Public Properties
    }
}