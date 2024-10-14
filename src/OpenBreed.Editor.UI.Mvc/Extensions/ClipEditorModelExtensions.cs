using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Editor.UI.Mvc.Models;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.Interface;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.Mvc.Extensions
{
    public static class ClipEditorModelExtensions
    {
        #region Public Methods

        public static MyExtentF GetExtent(this IClipEditorModel model)
        {
            if (model.Track is null)
            {
                return new MyExtentF();
            }

            var extent = GetExtent(model.Track);

            extent.Expand(model.ClipLength, extent.Center.Y);

            return extent;
        }

        public static MyExtentF GetExtent(IDbAnimationTrack track)
        {
            switch (track)
            {
                case IDbAnimationTrack<int> intTrack:
                    return GetExtent(intTrack);

                case IDbAnimationTrack<string> stringTrack:
                    return GetExtent(stringTrack);

                default:
                    throw new NotImplementedException("Track type not implemented");
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static MyExtentF GetExtent(IDbAnimationTrack<string> track)
        {
            return new MyExtentF();
        }

        private static MyExtentF GetExtent(IDbAnimationTrack<int> track)
        {
            var extent = new MyExtentF();

            if (track.Frames.Count == 0)
            {
                return extent;
            }

            foreach (var frame in track.Frames)
            {
                extent.Expand(frame.Time, frame.Value);
            }

            return extent;
        }

        #endregion Private Methods
    }
}