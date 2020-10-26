using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBreed.Common.UI.WinForms.Controls
{
    class ToolStripButtonEx : ToolStripButton, IBindableComponent
    {
        #region Private Fields

        private BindingContext _bindingContext;
        private ControlBindingsCollection _dataBindings;

        #endregion Private Fields

        #region Public Properties

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

        public new bool Enabled
        {
            get
            {
                return base.Enabled;
            }

            set
            {
                if (Parent != null)
                    Parent.InvokeIfRequired(() => { base.Enabled = value; });
            }
        }

        #endregion Public Properties
    }
}
