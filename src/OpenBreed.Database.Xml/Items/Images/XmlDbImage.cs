using OpenBreed.Database.Xml.Items.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;
using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Xml.Items.Sounds;
using System.Threading.Channels;

namespace OpenBreed.Database.Xml.Items.Images
{
    [Serializable]
    public abstract class XmlDbImage : XmlDbEntry, IDbImage
    {
        #region Protected Constructors

        protected XmlDbImage()
        {
        }

        protected XmlDbImage(XmlDbImage other) : base(other)
        {
        }

        #endregion Protected Constructors
    }
}