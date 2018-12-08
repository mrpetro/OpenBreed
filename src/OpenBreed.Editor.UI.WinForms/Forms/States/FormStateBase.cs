using OpenBreed.Editor.UI.WinForms.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.WinForms.Forms.States
{
    internal abstract class FormState
    {
        #region Private Fields

        internal readonly MainForm MainForm;

        #endregion Private Fields

        #region Internal Constructors

        internal FormState(MainForm mainForm)
        {
            MainForm = mainForm;
        }

        internal abstract void Setup();
        internal abstract void Cleanup();

        #endregion Internal Constructors
    }
}
