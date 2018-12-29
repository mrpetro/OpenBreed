using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Common.Database.Items.Sources
{
    [Serializable]
    public class SourceDef : DatabaseItemDef, ISourceEntity
    {
        #region Public Properties

        public long Id { get; set; }

        #endregion Public Properties
    }
}
