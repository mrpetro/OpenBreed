using OpenBreed.Database.Interface.Items.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Actions
{
    public class XmlDbActionPresentation : IDbActionPresentation
    {
        #region Public Constructors

        public XmlDbActionPresentation()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbActionPresentation(XmlDbActionPresentation other)
        {
            Color = other.Color;
            Image = other.Image;
            Visibility = other.Visibility;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("Color")]
        public string Color { get; set; }

        [XmlElement("Image")]
        public string Image { get; set; }

        [XmlElement("Visibility")]
        public bool Visibility { get; set; }

        #endregion Public Properties

        #region Public Methods

        public IDbActionPresentation Copy() => new XmlDbActionPresentation(this);

        #endregion Public Methods
    }
}