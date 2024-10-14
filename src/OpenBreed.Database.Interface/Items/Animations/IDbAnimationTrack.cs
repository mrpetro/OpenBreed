using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Animations
{
    public interface IDbAnimationTrack
    {
        #region Public Properties

        EntryFrameInterpolation Interpolation { get; }

        string Controller { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Copy this object
        /// </summary>
        /// <returns>Copy of this object</returns>
        IDbAnimationTrack Copy();

        #endregion Public Methods
    }

    public interface IDbAnimationTrack<TValue> : IDbAnimationTrack
    {
        #region Public Properties

        ReadOnlyCollection<IDbAnimationFrame<TValue>> Frames { get; }

        #endregion Public Properties

        #region Public Methods

        void ClearFrames();

        void AddFrame(TValue value, float frameTime);

        bool RemoveFrame(IDbAnimationFrame frame);

        #endregion Public Methods
    }
}