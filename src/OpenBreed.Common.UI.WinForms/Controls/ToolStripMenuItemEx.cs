﻿using OpenBreed.Common.UI.WinForms.Extensions;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenBreed.Common.UI.WinForms.Controls
{
    public class ToolStripMenuItemEx : ToolStripMenuItem, IBindableComponent
    {

        #region Private Fields

        private BindingContext _bindingContext;
        private ControlBindingsCollection _dataBindings;

        #endregion Private Fields

        #region Public Constructors

        public ToolStripMenuItemEx(string text) : base(text)
        {
        }

        #endregion Public Constructors

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
                if(Parent != null)
                    Parent.InvokeIfRequired(() => { base.Enabled = value; });
                else
                    base.Enabled = value;
            }
        }

        public new bool Checked
        {
            get
            {
                return base.Checked;
            }

            set
            {
                if (Parent != null)
                    Parent.InvokeIfRequired(() => { base.Checked = value; });
                else
                    base.Checked = value;
            }
        }

        #endregion Public Properties

    }
}