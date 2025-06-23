using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
    public class AnimationEditorVM : EntrySpecificEditorVM<IDbAnimation>, IClipEditorModel
    {
        #region Private Fields

        private ClipTrackItemVM selectedTrack;

        private ClipTrackPropertiesEditorVM trackPropertiesEditor;
        private readonly IServiceProvider serviceProvider;
        private readonly IAnimationSandbox animationSandbox;

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
            IAnimationSandboxFactory animationSandboxFactory) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
            this.serviceProvider = serviceProvider;
            this.animationSandbox = animationSandboxFactory.Create();

            this.Preview = ActivatorUtilities.CreateInstance<AnimationPreviewVM>(serviceProvider, animationSandbox);
            this.CurvesEditor = ActivatorUtilities.CreateInstance<AnimationCurvesEditorVM>(serviceProvider, animationSandbox);
            this.Player = ActivatorUtilities.CreateInstance<AnimationPlayerVM>(serviceProvider, animationSandbox); ;

            RestoreTracks();

            Preview.View(Entry);

            AddNewTrackCommand = new Command(() => AddNewTrack());
            CopyTrackCommand = new Command(() => CopyTrack(SelectedTrack.Source));
            RemoveTrackCommand = new Command(() => RemoveTrack(SelectedTrack.Source));


        }

        #endregion Public Constructors

        #region Public Properties

        public ClipTrackPropertiesEditorVM TrackPropertiesEditor
        {
            get { return trackPropertiesEditor; }
            set { SetProperty(ref trackPropertiesEditor, value); }
        }

        public float ClipLength
        {
            get { return Entry.Length; }
            set { SetProperty(Entry, x => x.Length, value); }
        }

        public ClipTrackItemVM SelectedTrack
        {
            get { return selectedTrack; }
            set { SetProperty(ref selectedTrack, value); }
        }

        public IReadOnlyCollection<IDbAnimationTrack> Tracks => Entry.Tracks;

        public ObservableCollection<ClipTrackItemVM> TrackItems { get; } = new ObservableCollection<ClipTrackItemVM>();

        public override string EditorName => "Animation editor";

        public ICommand AddNewTrackCommand { get; }

        public ICommand CopyTrackCommand { get; }

        public ICommand RemoveTrackCommand { get; }

        public IDbAnimationTrack Track => SelectedTrack?.Source;

        #endregion Public Properties

        #region Protected Methods



        internal void EditTrack(IDbAnimationTrack dbTrack)
        {
            TrackPropertiesEditor = ActivatorUtilities.CreateInstance<ClipTrackPropertiesEditorVM>(serviceProvider,
                                                                                                   dbTrack,
                                                                                                   OnTrackPropertyChanged);

            CurvesEditor.Edit(dbTrack);
        }

        private void OnTrackPropertyChanged(string propertyName)
        {
            SelectedTrack?.Refresh();
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(SelectedTrack):

                    EditTrack(SelectedTrack?.Source);
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void AddNewTrack()
        {
            var track = Entry.AddNewTrack<int>();
            TrackItems.Add(new ClipTrackItemVM(track));
        }

        private void CopyTrack(IDbAnimationTrack source)
        {
            throw new NotImplementedException();
        }

        private void RemoveTrack(IDbAnimationTrack source)
        {
            Entry.RemoveTrack(source);
            var trackVm = TrackItems.First(item => item.Source == source);
            TrackItems.Remove(trackVm);
        }

        private void RestoreTracks()
        {
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