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

        #region Public Methods

        public abstract IDbAnimationFrame Copy();

        #endregion Public Methods
    }

    public class XmlDbAnimationFrame<TValue> : XmlDbAnimationFrame, IDbAnimationFrame<TValue>
    {
        #region Public Properties

        [XmlAttribute]
        public TValue Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbAnimationFrame Copy()
        {
            return new XmlDbAnimationFrame<TValue>
            {
                Value = this.Value
            };
        }

        #endregion Public Methods
    }
}