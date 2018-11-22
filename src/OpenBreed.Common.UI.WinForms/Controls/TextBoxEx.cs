using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.UI.WinForms.Controls
{
    public class TextBoxEx : System.Windows.Forms.TextBox
    {
        private string m_LastValidText = null;

        /// <summary>
        ///Events
        /// </summary>
        public event ValidTextChangedEventHandler ValidTextChanged;

        public string LastValidText
        {
            get
            {
                return m_LastValidText;
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                m_LastValidText = base.Text;
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            //init storage...
            m_LastValidText = Text;
        }

        protected override void OnValidated(EventArgs e)
        {
            base.OnValidated(e);

            if (m_LastValidText != Text)
            {
                m_LastValidText = Text;
                OnValidTextChanged(new EventArgs());
            }
        }

        protected virtual void OnValidTextChanged(EventArgs e)
        {
            if(ValidTextChanged != null) ValidTextChanged(this,e);
        }
    }

    public delegate void ValidTextChangedEventHandler(object sender, EventArgs e);

}
