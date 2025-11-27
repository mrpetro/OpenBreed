using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Editor.UI.Mvc;
using OpenBreed.Editor.UI.Mvc.Controllers;
using OpenBreed.Editor.UI.Mvc.Models;
using OpenBreed.Editor.UI.Mvc.Views;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Rendering.Abstractions.Data;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Factories;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Wecs.Entities;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Animations
{
    public class AnimationEditorVM : EntrySpecificEditorVM<IDbAnimation>
    {
        #region Private Fields

        private readonly IServiceProvider serviceProvider;
        private readonly IFrameUpdaterMan<IEntity> frameUpdaterMan;
        private readonly IAnimationSandbox animationSandbox;
        private readonly IAnimationEditorModel editorModel;
        private ClipTrackItemVM selectedTrack;

        private float clipLength;
        private bool isSetClipLengthVisible;
        private bool isAddTrackModeEnabled;

        #endregion Private Fields

        #region Public Constructors

        public AnimationEditorVM(
            IDbAnimation dbEntry,
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            IServiceProvider serviceProvider,
            IFrameUpdaterMan<IEntity> frameUpdaterMan,
            IAnimationSandboxFactory animationSandboxFactory) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
            this.serviceProvider = serviceProvider;
            this.frameUpdaterMan = frameUpdaterMan;
            this.animationSandbox = animationSandboxFactory.Create();

            this.editorModel = ActivatorUtilities.CreateInstance<AnimationEditorModel>(serviceProvider, dbEntry);
            this.Preview = ActivatorUtilities.CreateInstance<AnimationPreviewVM>(serviceProvider, animationSandbox, editorModel);
            this.CurvesEditor = ActivatorUtilities.CreateInstance<AnimationCurvesEditorVM>(serviceProvider, animationSandbox, editorModel);
            this.Player = ActivatorUtilities.CreateInstance<AnimationPlayerVM>(serviceProvider, animationSandbox);
            ComponentSelector = new AnimationComponentSelectorVM(frameUpdaterMan, OnAnimationSelectorConfirm, OnAnimationSelectorCancel);

            Restore();

            AddNewTrackCommand = new Command(() => AddNewTrack());
            SetClipLengthCommand = new Command(() => SetClipLength());
            RemoveTrackCommand = new Command(() => RemoveTrack(SelectedTrack.Source));

            IsAddTrackModeEnabled = true;
        }

        #endregion Public Constructors

        #region Public Properties

        public AnimationCurvesEditorVM CurvesEditor { get; }
        public AnimationPreviewVM Preview { get; }
        public AnimationPlayerVM Player { get; }

        public float ClipLength
        {
            get { return clipLength; }
            set { SetProperty(ref clipLength, value); }
        }

        public bool IsAddTrackModeEnabled
        {
            get { return isAddTrackModeEnabled; }
            set { SetProperty(ref isAddTrackModeEnabled, value); }
        }

        public bool IsSetClipLengthVisible
        {
            get { return isSetClipLengthVisible; }
            set { SetProperty(ref isSetClipLengthVisible, value); }
        }

        public ClipTrackItemVM SelectedTrack
        {
            get { return selectedTrack; }
            set { SetProperty(ref selectedTrack, value); }
        }

        public ObservableCollection<ClipTrackItemVM> TrackItems { get; } = new ObservableCollection<ClipTrackItemVM>();

        public AnimationComponentSelectorVM ComponentSelector { get; }

        public override string EditorName => "Animation editor";

        public ICommand AddNewTrackCommand { get; }
        public ICommand SetClipLengthCommand { get; }
        public ICommand RemoveTrackCommand { get; }

        #endregion Public Properties

        #region Internal Methods

        internal void EditTrack(IDbAnimationTrack dbTrack)
        {
            CurvesEditor.Edit(dbTrack);
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void ProtectedUpdateEntry()
        {
            editorModel.Store();

            base.ProtectedUpdateEntry();
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(SelectedTrack):

                    EditTrack(SelectedTrack?.Source);
                    break;

                case nameof(ClipLength):
                    OnClipLengthChanging();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void OnTrackPropertyChanged(string propertyName)
        {
            SelectedTrack?.Refresh();
        }

        private void OnClipLengthChanging()
        {
            IsSetClipLengthVisible = ClipLength != editorModel.ClipLength;
        }

        private void OnAnimationSelectorCancel()
        {
            IsAddTrackModeEnabled = true;
        }

        private void OnAnimationSelectorConfirm(string animatorName)
        {
            var track = Entry.AddNewTrack<int>(animatorName);
            TrackItems.Add(new ClipTrackItemVM(track));

            IsAddTrackModeEnabled = true;
        }

        private void AddNewTrack()
        {
            IsAddTrackModeEnabled = false;

            ComponentSelector.Start(TrackItems.Select(item => item.Source.Controller));
        }

        private void SetClipLength()
        {
            editorModel.ClipLength = ClipLength;
            OnClipLengthChanging();
        }

        private void RemoveTrack(IDbAnimationTrack source)
        {
            editorModel.RemoveTrack(source);
            var trackVm = TrackItems.First(item => item.Source == source);
            TrackItems.Remove(trackVm);
        }

        private void Restore()
        {
            ClipLength = editorModel.ClipLength;

            TrackItems.Clear();

            foreach (var item in Entry.Tracks)
            {
                var itemVm = new ClipTrackItemVM(item);

                TrackItems.Add(itemVm);
            }
        }

        #endregion Private Methods
    }
}