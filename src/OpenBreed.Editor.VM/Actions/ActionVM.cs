using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Actions;
using System;

namespace OpenBreed.Editor.VM.Actions
{
    public delegate bool VisibilityChangedEventHandler(ActionVM action, bool isVisible);

    public class ActionVM : BaseViewModel
    {
        #region Private Fields

        private MyColor color;
        private string description;
        private int id;
        private IImage icon;
        private bool isVisible;
        private string name;
        private readonly IDrawingFactory drawingFactory;
        private readonly IDrawingContextProvider drawingContextProvider;
        private ActionModel model;
        private readonly Action<ActionVM> changeCallback;

        #endregion Private Fields

        #region Public Constructors

        public ActionVM(
            IDrawingFactory drawingFactory,
            IDrawingContextProvider drawingContextProvider,
            ActionModel model,
            Action<ActionVM> changeCallback)
        {
            this.drawingFactory = drawingFactory;
            this.drawingContextProvider = drawingContextProvider;
            this.model = model;
            this.changeCallback = changeCallback;

            Restore();
        }

        #endregion Public Constructors

        #region Public Properties

        public VisibilityChangedEventHandler VisibilityChanged { get; set; }

        public MyColor Color
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

        public IImage Icon
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
                    ActionSetsDataHelper.SetPresentationDefault(drawingFactory, drawingContextProvider, model, Color);
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
                    VisibilityChanged?.Invoke(this, IsVisible);
                    break;

                case nameof(Id):
                    model.Id = Id;
                    break;

                default:
                    break;
            }

            changeCallback?.Invoke(this);

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods
    }
}