using System;
using System.Windows.Forms;

namespace OpenBreed.Common.UI.WinForms.Controls
{
    public class ComboBoxEx : ComboBox
    {
        #region Protected Methods

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            //This will cause selected item value change instantly after index is changed
            if (DataBindings.Count > 0)
                DataBindings[0].WriteValue();

            base.OnSelectedIndexChanged(e);
        }

        #endregion Protected Methods
    }
}