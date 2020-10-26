using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBreed.Common.UI.WinForms.Controls
{
    /// <summary>
    /// This class extends ToolStripStatusLabel control with usage of DataBindings features
    /// </summary>
    public class ToolStripStatusLabelEx : ToolStripStatusLabel, IBindableComponent
    {
        private BindingContext _bindingContext;
        private ControlBindingsCollection _dataBindings;

        [Browsable(false)]
        public BindingContext BindingContext
        {
            get
            {
                if (this._bindingContext == null)
                    this._bindingContext = new BindingContext();

                return this._bindingContext;
            }
            set
            {
                this._bindingContext = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlBindingsCollection DataBindings
        {
            get
            {
                if (this._dataBindings == null)
                    this._dataBindings = new ControlBindingsCollection(this);

                return this._dataBindings;
            }
        }

        public new string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                if (Parent != null)
                    Parent.InvokeIfRequired(() => { base.Text = value; });
            }
        }

        public new Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }

            set
            {
                if (Parent != null)
                    Parent.InvokeIfRequired(() => { base.ForeColor = value; });
            }
        }

    }
}
