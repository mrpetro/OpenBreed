using OpenBreed.Database.Interface.Items.Animations;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Animations
{
    public abstract class XmlDbAnimationFrame : IDbAnimationFrame
    {
        #region Public Properties

        [XmlAttribute]
        public float Time { get; set; }

        #endregion Public Properties
    }

    public class XmlDbAnimationFrame<TValue> : XmlDbAnimationFrame, IDbAnimationFrame<TValue>
    {
        #region Public Properties

        [XmlAttribute]
        public TValue Value { get; set; }

        #endregion Public Properties
    }

}