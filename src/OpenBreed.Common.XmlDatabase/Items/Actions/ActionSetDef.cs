using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Actions;

namespace OpenBreed.Common.XmlDatabase.Items.Actions
{
    [Serializable]
    public class ActionSetDef : DatabaseItemDef, IActionSetEntry
    {

        #region Public Properties

        [XmlArray("Actions")]
        [XmlArrayItem("Action")]
        public List<ActionDef> Actions { get; } = new List<ActionDef>();

        [XmlIgnore]
        public List<IActionEntry> Items
        {
            get
            {
                return Actions.OfType<IActionEntry>().ToList();
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override IEntry Copy()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }
}
