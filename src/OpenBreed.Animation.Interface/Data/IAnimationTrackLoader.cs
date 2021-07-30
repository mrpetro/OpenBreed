using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Animations;
using System;

namespace OpenBreed.Animation.Interface.Data
{
    public interface IAnimationTrackLoader
    {
        #region Public Methods

        void Load(IClip animation, IDbAnimationTrack animationEntryPart);

        #endregion Public Methods
    }
}