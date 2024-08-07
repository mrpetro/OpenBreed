﻿using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Xml.Items.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml
{
    public abstract class XmlDbEntry : IDbEntry
    {
        #region Protected Constructors

        protected XmlDbEntry()
        {
        }

        protected XmlDbEntry(XmlDbEntry other)
        {
            Id = other.Id;
            Description = other.Description;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlAttribute]
        public string Description { get; set; }

        [XmlAttribute]
        public string Id { get; set; }

        #endregion Public Properties

        #region Public Methods

        public abstract IDbEntry Copy();

        public bool Equals(IDbEntry other)
        {
            if (other is null)
            {
                return false;
            }

            if (!Equals(Id, other.Id))
            {
                return false;
            }

            if (!Equals(Description, other.Description))
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"DbTableItem '{Id}'";
        }

        #endregion Public Methods
    }
}