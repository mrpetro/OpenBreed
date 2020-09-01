﻿using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Scripts;
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
    [Description("Script from file"), Category("Appearance")]
    public class XmlScriptFromFileEntry : XmlScriptEntry, IScriptFromFileEntry
    {
        #region Public Properties

        #endregion Public Properties

        #region Public Methods

        public override IEntry Copy()
        {
            return new XmlScriptEmbeddedEntry()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef
            };
        }

        #endregion Public Methods
    }
}
