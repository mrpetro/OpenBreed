using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Database.Xml.Items.EntityTemplates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Scripts
{
    [Serializable]
    public abstract class XmlDbScript : XmlDbEntry, IDbScript
    {
        #region Protected Constructors

        protected XmlDbScript()
        {
        }

        protected XmlDbScript(XmlDbScript other) : base(other)
        {
        }

        #endregion Protected Constructors
    }
}