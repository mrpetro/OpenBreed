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
using OpenBreed.Database.Interface.Comparer;

namespace OpenBreed.Database.Xml.Items.Actions
{
    [Serializable]
    [Description("Action set"), Category("Appearance")]
    public class XmlDbActionSet : XmlDbEntry, IDbActionSet, IEquatable<IDbActionSet>
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

            init
            {
                _xmlActions = value;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy()
        {
            return new XmlDbActionSet
            {
                XmlActions = this.XmlActions
            };
        }

        public bool Equals(IDbActionSet other)
        {
            if (!base.Equals(other))
            {
                return false;
            }

            if (!Actions.SequenceEqual(other.Actions, DbActionComparer.Instance))
            {
                return false;
            }

            return true;
        }

        public IDbAction NewItem()
        {
            return new XmlDbAction();
        }

        #endregion Public Methods

    }
}
