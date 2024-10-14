using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Editor.VM.Base;
using System;

namespace OpenBreed.Editor.VM.Animations
{
    public class ClipTrackPropertiesEditorVM : BaseViewModel
    {
        #region Private Fields

        private readonly IDbAnimationTrack track;
        private readonly Action<string> propertyChangedCallback;

        #endregion Private Fields

        #region Public Constructors

        public ClipTrackPropertiesEditorVM(IDbAnimationTrack track, Action<string> propertyChangedCallback)
        {
            this.track = track;
            this.propertyChangedCallback = propertyChangedCallback;
        }

        #endregion Public Constructors

        #region Public Properties

        public string ControllerName
        {
            get { return track.Controller; }
            set { SetProperty(track, x => x.Controller, value); }
        }

        protected override void OnPropertyChanged(string name)
        {
            propertyChangedCallback?.Invoke(name);

            base.OnPropertyChanged(name);
        }

        #endregion Public Properties
    }
}