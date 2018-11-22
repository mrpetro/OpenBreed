using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace OpenBreed.Editor.UI.WinForms.Views
{
    public partial class PropertiesView : DockContent
    {
        public PropertiesView()
        {
            InitializeComponent();
        }

        public object SelectedObject
        {
            get
            {
                return PropertyGrid.SelectedObject;
            }

            set
            {
                PropertyGrid.SelectedObject = value;
            }
        }
    }
}
