using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Core.Interface.Managers;
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

        private ClipTrackItemVM selectedTrack;

        private float clipLength;
        private bool isSetClipLengthVisible;
        private bool isAddTrackModeEnabled;
        private readonly IServiceProvider serviceProvider;
        private readonly IFrameUpdaterMan<IEntity> frameUpdaterMan;
        private readonly IAnimationSandbox animationSandbox;
        private readonly IAnimationEditorModel model;

        public AnimationCurvesEditorVM CurvesEditor { get; }
        public AnimationPreviewVM Preview { get; }
        public AnimationPlayerVM Player { get; }

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

            this.model = ActivatorUtilities.CreateInstance<AnimationEditorModel>(serviceProvider, dbEntry);
            this.Preview = ActivatorUtilities.CreateInstance<AnimationPreviewVM>(serviceProvider, animationSandbox, model);
            this.CurvesEditor = ActivatorUtilities.CreateInstance<AnimationCurvesEditorVM>(serviceProvider, animationSandbox, model);
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

        #region Protected Methods

        internal void EditTrack(IDbAnimationTrack dbTrack)
        {
            model.Edit(dbTrack);
            CurvesEditor.Edit(dbTrack);
        }

        private void OnTrackPropertyChanged(string propertyName)
        {
            SelectedTrack?.Refresh();
        }

        private void OnClipLengthChanging()
        {
            IsSetClipLengthVisible = ClipLength != model.ClipLength;
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
            model.ClipLength = ClipLength;
            OnClipLengthChanging();
        }

        private void RemoveTrack(IDbAnimationTrack source)
        {
            Entry.RemoveTrack(source);
            var trackVm = TrackItems.First(item => item.Source == source);
            TrackItems.Remove(trackVm);
        }

        private void Restore()
        {
            ClipLength = model.ClipLength;

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