using OpenBreed.Editor.UI.WinForms.Forms;
using OpenBreed.Editor.UI.WinForms.Views;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Database.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBreed.Editor.UI.WinForms
{
    public class DialogProvider : IDialogProvider
    {
        private MainForm _mainForm;

        private MessageBoxButtons ToMessageBoxButtons(QuestionDialogButtons buttons)
        {
            switch (buttons)
            {
                case QuestionDialogButtons.OK:
                    return MessageBoxButtons.OK;
                case QuestionDialogButtons.OKCancel:
                    return MessageBoxButtons.OKCancel;
                case QuestionDialogButtons.AbortRetryIgnore:
                    return MessageBoxButtons.AbortRetryIgnore;
                case QuestionDialogButtons.YesNoCancel:
                    return MessageBoxButtons.YesNoCancel;
                case QuestionDialogButtons.YesNo:
                    return MessageBoxButtons.YesNo;
                case QuestionDialogButtons.RetryCancel:
                    return MessageBoxButtons.RetryCancel;
                default:
                    throw new InvalidOperationException("Unknown QuestionDialogButtons value conversion!");
            }
        }

        internal static DialogAnswer ToDialogAnswer(DialogResult result)
        {
            switch (result)
            {
                case DialogResult.None:
                    return DialogAnswer.None;
                case DialogResult.OK:
                    return DialogAnswer.OK;
                case DialogResult.Cancel:
                    return DialogAnswer.Cancel;
                case DialogResult.Abort:
                    return DialogAnswer.Abort;
                case DialogResult.Retry:
                    return DialogAnswer.Retry;
                case DialogResult.Ignore:
                    return DialogAnswer.Ignore;
                case DialogResult.Yes:
                    return DialogAnswer.Yes;
                case DialogResult.No:
                    return DialogAnswer.No;
                default:
                    throw new InvalidOperationException("Unknown DialogResult value conversion!");
            }
        }

        public void ShowMessage(string text, string caption)
        {
            MessageBox.Show(text, caption);
        }

        public DialogAnswer ShowReplaceFileQuestion(string text, string caption)
        {
            return ToDialogAnswer(MessageBox.Show(text, caption, ToMessageBoxButtons(QuestionDialogButtons.YesNoCancel)));
        }

        public DialogAnswer ShowMessageWithQuestion(string text, string caption, QuestionDialogButtons buttons)
        {
            return ToDialogAnswer(MessageBox.Show(text, caption, ToMessageBoxButtons(buttons)));
        }

        public IFolderBrowserQuery FolderBrowserDialog()
        {
            return new FolderBrowserQuery();
        }

        public IOpenFileQuery OpenFileDialog()
        {
            return new OpenFileQuery();
        }

        public ISaveFileQuery SaveFileDialog()
        {
            return new SaveFileQuery();
        }

        public void ShowEditorView(EditorVM editor)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _mainForm = new MainForm();
            _mainForm.Initialize(editor);
            Application.Run(_mainForm);
        }
    }
}
