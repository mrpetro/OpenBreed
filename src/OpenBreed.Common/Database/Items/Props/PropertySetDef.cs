using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using OpenBreed.Common.Logging;

namespace OpenBreed.Common.Database.Items.Props
{
    [Serializable]
    public class PropertySetDef : DatabaseItemDef
    {
        #region Public Properties

        public List<PropertyDef> PropertyDefs { get; } = new List<PropertyDef>();

        #endregion Public Properties

    }
}
