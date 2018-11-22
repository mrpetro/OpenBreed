using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using OpenBreed.Editor.VM;
using OpenBreed.Common.UI.WinForms.Controls;

namespace OpenABEd.Modules.MapEditor.Controls
{
    public partial class MapPropertiesCtrl : UserControl
    {
        public MapPropertiesCtrl()
        {
            InitializeComponent();
        }

        private void TextBoxEx_ValidTextChanged(object sender, EventArgs e)
        {
            btnApply.Enabled = true;
            btnCancel.Enabled = true;
        }

        void TextBoxEx_UInt32Validating(object sender, CancelEventArgs e)
        {
            TextBoxEx textBox = sender as TextBoxEx;

            UInt32 numberEntered;

            if (!UInt32.TryParse(textBox.Text, out numberEntered))
            {
                MessageBox.Show("You need to enter an UInt32 number.");
                textBox.Text = textBox.LastValidText;
            }
        }

        void TextBoxEx_UInt16Validating(object sender, CancelEventArgs e)
        {
            TextBoxEx textBox = sender as TextBoxEx;

            UInt16 numberEntered;

            if (!UInt16.TryParse(textBox.Text, out numberEntered))
            {
                MessageBox.Show("You need to enter an UInt16 number.");
                textBox.Text = textBox.LastValidText;
            }
        }


        void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        void btnApply_Click(object sender, EventArgs e)
        {
            Apply();
        }

        void Cancel()
        {
            if (!btnCancel.Enabled)
                throw new Exception("You should not be here.");

            btnCancel.Enabled = false;
            btnApply.Enabled = false;

        }

        void Apply()
        {
            if (!btnApply.Enabled)
                throw new Exception("You should not be here.");

            btnCancel.Enabled = false;
            btnApply.Enabled = false;

        }

        private void PrepareTextBoxValidTextChanged(Control control, EventHandler eventHandler)
        {
            if (control is TextBoxEx)
                ((TextBoxEx)control).ValidTextChanged += TextBoxEx_ValidTextChanged;
            else
                foreach (Control subControl in control.Controls)
                    PrepareTextBoxValidTextChanged(subControl, eventHandler);
        }

        private void Prepare()
        {

        }


        private void OpenMAPBtn_Click(object sender, EventArgs e)
        {

        }

    }
}
