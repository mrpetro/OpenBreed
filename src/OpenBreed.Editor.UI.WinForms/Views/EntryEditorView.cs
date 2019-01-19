using OpenBreed.Editor.UI.WinForms.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.WinForms.Views
{
    public class EntryEditorView<T> : EntryEditorBaseView where T : EntryEditorInnerCtrl, new()
    {
        #region Public Constructors

        public EntryEditorView()
        {
            EntryEditor.InnerCtrl = new T();

        }

        #endregion Public Constructors
    }
}
