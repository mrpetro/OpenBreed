using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Actions;

namespace OpenBreed.Editor.VM.Props
{
    public class PropVM : BaseViewModel
    {

        #region Private Fields

        private string _description;
        private int _id;
        private bool _isVisible;
        private string _name;
        private Image _presentation;

        #endregion Private Fields

        #region Public Constructors

        public PropVM(ActionSetVM owner)
        {
            Owner = owner;
        }

        #endregion Public Constructors

        #region Public Properties

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

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public ActionSetVM Owner { get; }
        public Image Presentation
        {
            get { return _presentation; }
            set { SetProperty(ref _presentation, value); }
        }

        public bool Visibility
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void FromModel(IActionEntry property)
        {
            Name = property.Name;
            Id = property.Id;
            Visibility = property.Presentation.Visibility;
            Description = property.Description;

            PropVMHelper.SetPresentation(this, property.Presentation);
        }

        #endregion Public Methods

    }
}
