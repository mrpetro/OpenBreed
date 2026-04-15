using OpenBreed.Animation.Abstractions;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.Mvc.Models
{
    public enum AnimationCurvesEditorMode
    {
        InsertKeyFrames,
        SelectKeyFrames,
        MoveKeyFrames
    }

    public interface IAnimationEditorModel : IEditorModel
    {
        #region Public Properties

        string Name { get; }

        float ClipLength { get; set; }

        IEditableTrack<IEntity> CurrentTrack { get; }

        AnimationCurvesEditorMode Mode { get; set; }

        IReadOnlyCollection<ITrackKeyFrame> SelectedKeyFrames { get; }
        ITrackKeyFrame? HoveredKeyFrame { get; set; }
        Vector2? AnchorPoint { get; }

        #endregion Public Properties

        #region Public Methods

        void Edit(IDbAnimationTrack dbTrack);
        void InsertKeyFrame(float time, float value);
        void RemoveTrack(IDbAnimationTrack source);
        void SelectKeyFrame(ITrackKeyFrame keyFrame);
        void ClearSelectedKeyFrames();
        void DeleteSelectedKeyFrames();
        bool TryGetClosestKeyFrame(Vector2 position, Vector2 tolerance, out ITrackKeyFrame? value);
        void StartSelecting(Vector2 anchorPoint);
        void CancelSelecting();
        void FinishSelecting();
        Vector2 GetStatingPosition(ITrackKeyFrame keyFrame);
        void MoveKeyFramesTo(Vector2 position);
        void MoveKeyFrameBy(ITrackKeyFrame keyFrame, Vector2 offsetPos);
        void FinishMoving();
        void Store();

        #endregion Public Methods
    }
}