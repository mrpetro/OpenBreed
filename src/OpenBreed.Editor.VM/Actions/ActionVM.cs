using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Actions;
using System.ComponentModel;

namespace OpenBreed.Editor.VM.Actions
{
    public class ActionVM : BaseViewModel
    {

        #region Private Fields

        private Color _color;
        private string _description;
        private int _id;
        private Image _icon;
        private bool _isVisible;
        private string _name;

        #endregion Private Fields

        #region Public Constructors

        public ActionVM(ActionSetVM owner)
        {
            Owner = owner;

            PropertyChanged += ActionVM_PropertyChanged;
        }

        private void ActionVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Color):
                    ActionVMHelper.SetPresentationDefault(this, Color);
                    break;
                default:
                    break;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public Color Color
        {
            get { return _color; }
            set { SetProperty(ref _color, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public Image Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public ActionSetVM Owner { get; }

        public bool Visibility
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void FromModel(IActionEntry action)
        {
            Name = action.Name;
            Id = action.Id;
            Description = action.Description;

            ActionVMHelper.FromModel(this, action.Presentation);
        }

        public void ToModel(IActionEntry action)
        {
            action.Name = Name;
            action.Id = Id;
            action.Description = Description;

            ActionVMHelper.ToModel(this, action.Presentation);
        }

        #endregion Public Methods

    }
}
