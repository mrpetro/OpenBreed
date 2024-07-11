using OpenBreed.Database.Interface.Items.Animations;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Animations
{
    public abstract class XmlDbAnimationFrame : IDbAnimationFrame
    {
        #region Protected Constructors

        protected XmlDbAnimationFrame()
        {
        }

        protected XmlDbAnimationFrame(XmlDbAnimationFrame other)
        {
            Time = other.Time;
        }

        #endregion Protected Constructors

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
        #region Public Constructors

        public XmlDbAnimationFrame()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbAnimationFrame(XmlDbAnimationFrame<TValue> other) : base(other)
        {
            Value = other.Value;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlAttribute]
        public TValue Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbAnimationFrame Copy() => new XmlDbAnimationFrame<TValue>(this);

        #endregion Public Methods
    }
}