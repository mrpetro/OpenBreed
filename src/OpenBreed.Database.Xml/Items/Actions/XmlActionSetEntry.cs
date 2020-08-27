using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Actions;
using System.ComponentModel;
using OpenBreed.Common;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Database.Interface.Items;

namespace OpenBreed.Database.Xml.Items.Actions
{
    [Serializable]
    [Description("Action set"), Category("Appearance")]
    public class XmlActionSetEntry : XmlDbEntry, IActionSetEntry
    {
        private List<IActionEntry> _actions = null;
        private List<ActionDef> _xmlActions = new List<ActionDef>();

        #region Public Properties

        [XmlIgnore]
        public List<IActionEntry> Actions
        {
            get
            {
                if (_actions == null)
                {
                    _actions = _xmlActions.Cast<IActionEntry>().ToList();
                    _xmlActions = null;
                }

                return _actions;
            }
        }

        [XmlArray("Actions")]
        [XmlArrayItem("Action")]
        public List<ActionDef> XmlActions
        {
            get
            {
                if (_actions != null)
                    _xmlActions = _actions.Cast<ActionDef>().ToList();

                return _xmlActions;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override IEntry Copy()
        {
            throw new NotImplementedException();
        }

        public IActionEntry NewItem()
        {
            return new ActionDef();
        }

        #endregion Public Methods

    }
}
