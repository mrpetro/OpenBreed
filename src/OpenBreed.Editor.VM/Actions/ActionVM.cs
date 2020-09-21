using OpenBreed.Common.Data;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Actions;
using System.Drawing;

namespace OpenBreed.Editor.VM.Actions
{
    public class ActionVM : BaseViewModel
    {
        #region Private Fields

        private Color color;
        private string description;
        private int id;
        private Image icon;
        private bool isVisible;
        private string name;

        private ActionModel model;

        #endregion Private Fields

        #region Public Constructors

        public ActionVM(ActionModel model)
        {
            this.model = model;

            Restore();
        }

        #endregion Public Constructors

        #region Public Properties

        public Color Color
        {
            get { return color; }
            set { SetProperty(ref color, value); }
        }

        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        public Image Icon
        {
            get { return icon; }
            set { SetProperty(ref icon, value); }
        }

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public bool IsVisible
        {
            get { return isVisible; }
            set { SetProperty(ref isVisible, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Restore()
        {
            color = model.Color;
            description = model.Description;
            id = model.Id;
            name = model.Name;
            icon = model.Icon;
            isVisible = model.Visibility;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(Color):
                    ActionSetsDataHelper.SetPresentationDefault(model, Color);
                    Icon = model.Icon;
                    model.Color = Color;
                    break;

                case nameof(Description):
                    model.Description = Description;
                    break;

                case nameof(Name):
                    model.Name = Name;
                    break;

                case nameof(IsVisible):
                    model.Visibility = IsVisible;
                    break;

                case nameof(Id):
                    model.Id = Id;
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods
    }
}