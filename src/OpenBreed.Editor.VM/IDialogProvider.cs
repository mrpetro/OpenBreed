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

        FolderBrowserResult ShowFolderBrowserDialog(string title, string initialDirectory);

        DialogAnswer ShowMessageWithQuestion(string text, string caption, QuestionDialogButtons buttons);

        FileDialogResult ShowOpenFileDialog(string title, string filter, bool multiSelect);
        FileDialogResult ShowOpenFileDialog(string title, string filter, string initialDirectory, string fileName = null);

        DialogAnswer ShowReplaceFileQuestion(string title, string caption);

        FileDialogResult ShowSaveFileDialog(string title, string filter, string initialDirectory, string fileName = null);

        #endregion Public Methods
    }

    public class FileDialogResult
    {
        #region Public Constructors

        public FileDialogResult(DialogAnswer answer, string[] fileNames)
        {
            Answer = answer;
            FileNames = fileNames;
        }

        #endregion Public Constructors

        #region Public Properties

        public DialogAnswer Answer { get; private set; }
        public string FileName { get { return FileNames.FirstOrDefault(); } }
        public string[] FileNames { get; private set; }

        #endregion Public Properties
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