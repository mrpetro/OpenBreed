using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Common.Props.Builders;

namespace OpenBreed.Common.Props
{
    public delegate void PropertyValueChangedEventHandler(object sender, PropertyValueChangedEventArgs e);

    public class PropertyValueChangedEventArgs : EventArgs
    {
        public PropertyModel Property { get; set; }

        public PropertyValueChangedEventArgs(PropertyModel property)
        {
            Property = property;
        }
    }

    public class PropertySetModel : IDisposable
    {
        private static PropertySetModel m_NullTileSet = null;

        public List<PropertyModel> Properties { get; set; }

        public event PropertyValueChangedEventHandler PropertyValueChanged;

        public PropertySetModel(PropertySetBuilder builder)
        {
            Properties = builder.Properties;

            foreach (var property in Properties)
                property.InternalOwner = this;
        }

        public static PropertySetModel NullTileSet
        {
            get
            {
                if (m_NullTileSet == null)
                {
                    m_NullTileSet = new PropertySetModel(PropertySetBuilder.NewPropertySet().SetDefaultProperties());
                    //m_NullTileSet.LoadFromDefault();
                }

                return m_NullTileSet;
            }
        }

        protected virtual void OnPropertyValueChanged(PropertyValueChangedEventArgs e)
        {
            if (PropertyValueChanged != null) PropertyValueChanged(this, e);
        }

        internal void OnInternalPropertyValueChanged(PropertyValueChangedEventArgs e)
        {
            OnPropertyValueChanged(e);
        }

        public void Dispose()
        {
        }
    }
}
