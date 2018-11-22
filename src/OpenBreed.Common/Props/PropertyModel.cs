using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Common.Props.Builders;
using System.Drawing;

namespace OpenBreed.Common.Props
{
    public class PropertyModel
    {
        private PropertySetModel m_Owner;

        internal int m_Id;
        internal bool m_Visibility;
        internal string m_Name;
        internal Image m_Presentation;
        internal string m_Description;

        internal PropertySetModel InternalOwner { set { m_Owner = value; } }

        public int Id
        {
            get { return m_Id; }

            set
            {
                if (m_Id != value)
                {
                    m_Id = value;
                    m_Owner.OnInternalPropertyValueChanged(new PropertyValueChangedEventArgs(this));
                }
            }
        }

        public bool Visibility
        { 
            get { return m_Visibility; }

            set
            {
                if (m_Visibility != value)
                {
                    m_Visibility = value;
                    m_Owner.OnInternalPropertyValueChanged(new PropertyValueChangedEventArgs(this));
                }
            }
        }

        public string Name
        {
            get { return m_Name; }

            set
            {
                if (m_Name != value)
                {
                    m_Name = value;
                    m_Owner.OnInternalPropertyValueChanged(new PropertyValueChangedEventArgs(this));
                }
            }
        }

        public Image Presentation
        {
            get { return m_Presentation; }

            set
            {
                if (m_Presentation != value)
                {
                    m_Presentation = value;
                    m_Owner.OnInternalPropertyValueChanged(new PropertyValueChangedEventArgs(this));
                }
            }
        }

        public string Description
        {
            get { return m_Description; }

            set
            {
                if (m_Description != value)
                {
                    m_Description = value;
                    m_Owner.OnInternalPropertyValueChanged(new PropertyValueChangedEventArgs(this));
                }
            }
        }

        public PropertyModel(PropertyBuilder builder)
        {
            m_Id = builder.Id;
            m_Visibility = builder.Visibility;
            m_Name = builder.Name;
            m_Presentation = builder.Presentation;
            m_Description = builder.Description;
        }
    }
}
