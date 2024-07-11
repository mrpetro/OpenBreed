using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Database.Xml.Items.Sprites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Texts
{
    [Serializable]
    public abstract class XmlDbText : XmlDbEntry, IDbText
    {
        #region Protected Constructors

        protected XmlDbText()
        {
        }

        protected XmlDbText(XmlDbText other) : base(other)
        {
        }

        #endregion Protected Constructors
    }
}