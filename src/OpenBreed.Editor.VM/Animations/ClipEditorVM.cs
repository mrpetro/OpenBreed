using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Editor.UI.Mvc.Controllers;
using OpenBreed.Editor.UI.Mvc.Models;
using OpenBreed.Editor.UI.Mvc.Views;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Factories;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Animations
{
    public class ClipEditorVM : EntrySpecificEditorVM<IDbAnimation>, IClipEditorModel
    {
        #region Private Fields

        private ClipTrackItemVM selectedTrack;

        private ClipTrackPropertiesEditorVM trackPropertiesEditor;
        private ClipEditorController renderViewController;
        private readonly IServiceProvider serviceProvider;
        private readonly IServiceScopeFactory serviceScopeFactory;

        #endregion Private Fields

        #region Public Constructors

        public ClipEditorVM(
            IDbAnimation dbEntry,
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            IServiceProvider serviceProvider,
            IServiceScopeFactory serviceScopeFactory) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
            this.serviceProvider = serviceProvider;
            this.serviceScopeFactory = serviceScopeFactory;

            RestoreTracks();

            AddNewTrackCommand = new Command(() => AddNewTrack());
            CopyTrackCommand = new Command(() => CopyTrack(SelectedTrack.Source));
            RemoveTrackCommand = new Command(() => RemoveTrack(SelectedTrack.Source));
        }

        #endregion Public Constructors

        #region Public Properties

        public Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext> InitFunc => OnInitialize;

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
        public IDbAnimationTrack Track { get; private set; }

        public override string EditorName => "Animation editor";

        public ICommand AddNewTrackCommand { get; }

        public ICommand CopyTrackCommand { get; }

        public ICommand RemoveTrackCommand { get; }

        #endregion Public Properties

        #region Protected Methods



        internal void Edit(IDbAnimationTrack dbTrack)
        {
            TrackPropertiesEditor = ActivatorUtilities.CreateInstance<ClipTrackPropertiesEditorVM>(serviceProvider, dbTrack, OnTrackPropertyChanged);
            Track = dbTrack;

            renderViewController.Reset();
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
                    Edit(SelectedTrack.Source);
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

        private IRenderContext OnInitialize(IGraphicsContext graphicsContext, HostCoordinateSystemConverter hostCoordinateSystemConverter)
        {
            var serviceScope = serviceScopeFactory.CreateScope();
            serviceScope.ServiceProvider.GetRequiredService<IRenderContextFactory>().SetupScope(hostCoordinateSystemConverter, graphicsContext);

            var renderContext = serviceScope.ServiceProvider.GetRequiredService<IRenderContext>();
            var eventsMan = serviceScope.ServiceProvider.GetRequiredService<IEventsMan>();

            var view = new EditorView(eventsMan, renderContext);
            view.SetScaleLimits(1.0f / (float)Math.Pow(2, 8), (float)Math.Pow(2, 8)); 

            renderViewController = ActivatorUtilities.CreateInstance<ClipEditorController>(serviceScope.ServiceProvider, view, this);

            if (Entry is not null)
            {
                eventsMan.Subscribe<RenderContextInitializedEvent>((rc) =>
                {
                    renderViewController.Reset();
                });
            }

            return renderContext;
        }

        #endregion Private Methods
    }
}