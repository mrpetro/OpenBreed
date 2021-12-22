using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;
using OpenBreed.Common;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Database.Interface.Items;

namespace OpenBreed.Database.Xml.Items.Actions
{
    [Serializable]
    [Description("Action set"), Category("Appearance")]
    public class XmlDbActionSet : XmlDbEntry, IDbActionSet
    {
        private List<IDbAction> _actions = null;
        private List<XmlDbAction> _xmlActions = new List<XmlDbAction>();

        #region Public Properties

        [XmlIgnore]
        public List<IDbAction> Actions
        {
            get
            {
                if (_actions == null)
                {
                    _actions = _xmlActions.Cast<IDbAction>().ToList();
                    _xmlActions = null;
                }

                return _actions;
            }
        }

        [XmlArray("Actions")]
        [XmlArrayItem("Action")]
        public List<XmlDbAction> XmlActions
        {
            get
            {
                if (_actions != null)
                    _xmlActions = _actions.Cast<XmlDbAction>().ToList();

                return _xmlActions;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy()
        {
            throw new NotImplementedException();
        }

        public IDbAction NewItem()
        {
            return new XmlDbAction();
        }

        #endregion Public Methods

    }
}
