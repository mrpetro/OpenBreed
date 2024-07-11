using OpenBreed.Common.Interface.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace OpenBreed.Common.Windows.Dialog
{
    public class DialogProvider : IDialogProvider
    {
        private MessageBoxButton ToMessageBoxButtons(QuestionDialogButtons buttons)
        {
            switch (buttons)
            {
                case QuestionDialogButtons.OK:
                    return MessageBoxButton.OK;
                case QuestionDialogButtons.OKCancel:
                    return MessageBoxButton.OKCancel;
                case QuestionDialogButtons.YesNoCancel:
                    return MessageBoxButton.YesNoCancel;
                case QuestionDialogButtons.YesNo:
                    return MessageBoxButton.YesNo;
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

        internal static DialogAnswer ToDialogAnswer(MessageBoxResult result)
        {
            switch (result)
            {
                case MessageBoxResult.None:
                    return DialogAnswer.None;
                case MessageBoxResult.OK:
                    return DialogAnswer.OK;
                case MessageBoxResult.Cancel:
                    return DialogAnswer.Cancel;
                case MessageBoxResult.Yes:
                    return DialogAnswer.Yes;
                case MessageBoxResult.No:
                    return DialogAnswer.No;
                default:
                    throw new InvalidOperationException("Unknown DialogResult value conversion!");
            }
        }

        public void ShowMessage(string text, string caption)
        {
            System.Windows.MessageBox.Show(text, caption);
        }

        public DialogAnswer ShowReplaceFileQuestion(string text, string caption)
        {
            return ToDialogAnswer(System.Windows.MessageBox.Show(text, caption, ToMessageBoxButtons(QuestionDialogButtons.YesNoCancel)));
        }

        public DialogAnswer ShowMessageWithQuestion(string text, string caption, QuestionDialogButtons buttons)
        {
            return ToDialogAnswer(System.Windows.MessageBox.Show(text, caption, ToMessageBoxButtons(buttons)));
        }

        public IFolderBrowserQuery FolderBrowserDialog()
        {
            throw new NotImplementedException("FolderBrowserDialog");
        }

        public IOpenFileQuery OpenFileDialog()
        {
            return new OpenFileQuery();
        }

        public ISaveFileQuery SaveFileDialog()
        {
            return new SaveFileQuery();
        }
    }
}
