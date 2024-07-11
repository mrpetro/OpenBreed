using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Tables
{
    public abstract class XmlDbTableDef
    {
        #region Public Methods

        [XmlIgnore]
        public abstract string Name { get; }

        public override string ToString()
        {
            return GetType().ToString();
        }

        #endregion Public Methods
    }
}