using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Editor.UI.Mvc.Models;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Animation.Abstractions;
using OpenBreed.Wecs.Abstractions.Primitives;

namespace OpenBreed.Editor.UI.Mvc.Extensions
{
    public static class ClipEditorModelExtensions
    {
        #region Public Methods

        public static MyExtentF GetExtent(this IAnimationEditorModel model)
        {
            if (model is null)
            {
                return MyExtentF.Empty;
            }

            if (model.CurrentTrack is null)
            {
                return MyExtentF.Empty;
            }

            var extent = GetExtent(model.CurrentTrack);

            extent.Expand(model.ClipLength, extent.Center.Y);

            return extent;
        }

        public static MyExtentF GetExtent(IReadOnlyTrack<IEntity> track)
        {
            switch (track)
            {
                case IReadOnlyTrack<IEntity, int> intTrack:
                    return GetExtent(intTrack);

                case IReadOnlyTrack<IEntity, string> stringTrack:
                    return GetExtent(stringTrack);

                default:
                    throw new NotImplementedException("Track type not implemented");
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static MyExtentF GetExtent(IReadOnlyTrack<IEntity, string> track)
        {
            return new MyExtentF();
        }

        private static MyExtentF GetExtent(IReadOnlyTrack<IEntity, int> track)
        {
            var extent = new MyExtentF();

            if (track.Frames.Count == 0)
            {
                return extent;
            }

            foreach (var frame in track.Frames)
            {
                extent.Expand(frame.Key, frame.Value);
            }

            return extent;
        }

        #endregion Private Methods
    }
}