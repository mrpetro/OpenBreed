using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Editor.UI.Mvc.Controllers;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.Mvc.Models
{
    public class AnimationEditorModel : IAnimationEditorModel
    {
        #region Private Fields

        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IDbAnimation dbEntry;
        private readonly IEditableClip<IEntity> model;
        private readonly Dictionary<ITrackKeyFrame, (float, object)> selectedKeyFrames = new Dictionary<ITrackKeyFrame, (float, object)>();

        #endregion Private Fields

        #region Public Constructors

        public AnimationEditorModel(IDataLoaderFactory dataLoaderFactory, IDbAnimation dbEntry)
        {
            this.dataLoaderFactory = dataLoaderFactory ?? throw new ArgumentNullException(nameof(dataLoaderFactory));
            this.dbEntry = dbEntry;
            var loader = dataLoaderFactory.GetLoader<IEditableClipDataLoader<IEntity>>();

            this.model = loader.Load(dbEntry);
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name => model.Name;

        public float ClipLength
        {
            get => model.Length;
            set
            {
                if (model.Length == value)
                {
                    return;
                }

                model.Length = value;
            }
        }

        public IEditableTrack<IEntity> CurrentTrack { get; private set; }

        public AnimationCurvesEditorMode Mode { get; set; } = AnimationCurvesEditorMode.SelectKeyFrames;
        public IReadOnlyCollection<ITrackKeyFrame> SelectedKeyFrames => selectedKeyFrames.Keys;
        public ITrackKeyFrame? HoveredKeyFrame { get; set; }
        public Vector2? AnchorPoint { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Edit(IDbAnimationTrack dbTrack)
        {
            CurrentTrack = model.GetTrack(dbTrack.Controller);
        }

        public void InsertKeyFrame(float time, float value)
        {
            if (CurrentTrack is not IEditableTrack<IEntity, int> intTrack)
            {
                return;
            }

            if (!intTrack.TryAddKeyFrame(time, (int)value))
            {
                return;
            }
        }

        public void RemoveTrack(IDbAnimationTrack dbTrack)
        {
            model.RemoveTrack(dbTrack.Controller);
        }

        public void SelectKeyFrame(ITrackKeyFrame keyFrame)
        {
            selectedKeyFrames.Add(keyFrame, (keyFrame.Key, keyFrame.GetValue()));
            //Mode = AnimationCurvesEditorMode.MoveKeyFrames;
        }

        public void ClearSelectedKeyFrames()
        {
            selectedKeyFrames.Clear();
        }

        public void DeleteSelectedKeyFrames()
        {
            foreach (var keyFrame in selectedKeyFrames)
            {
                CurrentTrack.RemoveFrames(selectedKeyFrames.Select(item => item.Key));
            }

            selectedKeyFrames.Clear();
        }

        public bool TryGetClosestKeyFrame(Vector2 position, Vector2 tolerance, out ITrackKeyFrame? keyFrame)
        {
            if (CurrentTrack is null)
            {
                keyFrame = null;
                return false;
            }

            var minTime = position.X - tolerance.X;
            var maxTime = position.X + tolerance.X;

            if (!CurrentTrack.TryFindInRange(minTime, maxTime, out IEnumerable<float> keyFrames))
            {
                keyFrame = null;
                return false;
            }

            if (CurrentTrack is not IEditableTrack<IEntity, int> currentTrack)
            {
                keyFrame = null;
                return false;
            }

            keyFrame = keyFrames.Select(item => currentTrack.GetKeyFrame(item)).FirstOrDefault(item => Math.Abs(position.Y - item.Value) < tolerance.Y);

            return keyFrame != null;
        }

        public Vector2 GetStatingPosition(ITrackKeyFrame keyFrame)
        {
            if (!selectedKeyFrames.TryGetValue(keyFrame, out (float, object) pos))
            {
                throw new InvalidOperationException("Expected key frame in selection collection.");
            }

            if (pos.Item2 is int y)
            {
                return new Vector2(pos.Item1, y);
            }

            throw new NotImplementedException();
        }

        public void StartSelecting(Vector2 anchorPoint)
        {
            AnchorPoint = anchorPoint;
        }

        public void CancelSelecting()
        {
            AnchorPoint = null;
        }

        public void FinishSelecting()
        {
            AnchorPoint = null;
        }

        public void MoveKeyFramesTo(Vector2 position)
        {
            var offsetPos = position - AnchorPoint.Value;

            foreach (var keyFrame in SelectedKeyFrames)
            {
                MoveKeyFrameBy(keyFrame, offsetPos);
            }
        }

        public void FinishMoving()
        {
            var keyFramesToUpdate = selectedKeyFrames.Keys.ToArray();
            foreach (var keyFrame in keyFramesToUpdate)
            {
                if (keyFrame is not ITrackKeyFrame<int> intKeyFrame)
                {
                    continue;
                }

                selectedKeyFrames[keyFrame] = (intKeyFrame.Key, intKeyFrame.Value);
            }

            //selectedKeyFrames[keyFrame] = (intKeyFrame.Key, intKeyFrame.Value);
        }

        public void MoveKeyFrameBy(ITrackKeyFrame keyFrame, Vector2 offsetPos)
        {
            if (keyFrame is not ITrackKeyFrame<int> intKeyFrame)
            {
                return;
            }

            var startPos = GetStatingPosition(keyFrame);

            intKeyFrame.Key = startPos.X + offsetPos.X;
            intKeyFrame.Value = (int)startPos.Y + (int)offsetPos.Y;
        }

        public void Store()
        {
            dbEntry.Length = model.Length;

            dbEntry.ClearTracks();

            foreach (var track in model.Tracks)
            {
                switch (track)
                {
                    case IReadOnlyTrack<IEntity, int> intTrack:
                        StoreTrack(intTrack);
                        break;

                    case IReadOnlyTrack<IEntity, float> floatTrack:
                        StoreTrack(floatTrack);
                        break;

                    case IReadOnlyTrack<IEntity, string> stringTrack:
                        StoreTrack(stringTrack);
                        break;

                    default:
                        break;
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void StoreTrack<TValue>(IReadOnlyTrack<IEntity, TValue> track)
        {
            var newTrack = dbEntry.AddNewTrack<TValue>(track.Id);

            newTrack.Controller = track.Id;

            foreach (var frame in track.Frames)
            {
                newTrack.AddFrame(frame.Key, frame.Value);
            }
        }

        #endregion Private Methods
    }
}