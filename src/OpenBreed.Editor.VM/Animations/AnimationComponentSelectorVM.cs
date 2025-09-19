using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Animations
{
    public class AnimationComponentSelectorVM : BaseViewModel
    {
        #region Private Fields

        private readonly IFrameUpdaterMan<IEntity> frameUpdaterMan;

        private readonly Action<string> onConfirmAction;
        private readonly Action onCancelAction;

        private bool isVisible;
        private bool isConfirmEnabled;
        private string selectedComponentName;
        private string selectedComponentPropertyName;

        #endregion Private Fields

        #region Public Constructors

        public AnimationComponentSelectorVM(IFrameUpdaterMan<IEntity> frameUpdaterMan, Action<string> onConfirmAction, Action onCancelAction)
        {
            this.frameUpdaterMan = frameUpdaterMan ?? throw new ArgumentNullException(nameof(frameUpdaterMan));
            this.onConfirmAction = onConfirmAction ?? throw new ArgumentNullException(nameof(onConfirmAction));
            this.onCancelAction = onCancelAction ?? throw new ArgumentNullException(nameof(onCancelAction));

            ConfirmCommand = new Command(() => Confirm());
            CancelCommand = new Command(() => Cancel());

            IsConfirmEnabled = ValidateIfConfirmEnabled();
        }

        #endregion Public Constructors

        #region Public Properties

        public bool IsVisible
        {
            get { return isVisible; }
            private set { SetProperty(ref isVisible, value); }
        }

        public bool IsConfirmEnabled
        {
            get { return isConfirmEnabled; }
            private set { SetProperty(ref isConfirmEnabled, value); }
        }

        public string SelectedComponentName
        {
            get { return selectedComponentName; }
            set { SetProperty(ref selectedComponentName, value); }
        }

        public string SelectedComponentPropertyName
        {
            get { return selectedComponentPropertyName; }
            set { SetProperty(ref selectedComponentPropertyName, value); }
        }

        public ObservableCollection<string> ComponentNames { get; } = new ObservableCollection<string>();

        public ObservableCollection<string> ComponentPropertyNames { get; } = new ObservableCollection<string>();

        public ICommand ConfirmCommand { get; }

        public ICommand CancelCommand { get; }

        #endregion Public Properties

        #region Public Methods

        public void Start(IEnumerable<string> existingTracks)
        {
            ComponentPropertyNames.Clear();

            var toAdd = frameUpdaterMan.AnimatorNames.Except(existingTracks);

            foreach (var item in toAdd)
            {
                ComponentPropertyNames.Add(item);
            }

            IsVisible = true;
        }

        #endregion Public Methods

        #region Private Methods

        private void Cancel()
        {
            onCancelAction.Invoke();
            IsVisible = false;
        }

        private bool ValidateIfConfirmEnabled()
        {
            return !string.IsNullOrWhiteSpace(SelectedComponentPropertyName);
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(SelectedComponentPropertyName):
                    IsConfirmEnabled = ValidateIfConfirmEnabled();
                    break;
                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        private void Confirm()
        {
            onConfirmAction.Invoke(SelectedComponentPropertyName);
            IsVisible = false;



        }

        #endregion Private Methods
    }
}