using OpenBreed.Common.Props;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        internal static PropVM Create(PropertyModel propertyModel)
        {
            var newProperty = new PropVM();
            newProperty.Name = propertyModel.Name;
            newProperty.Id = propertyModel.Id;
            newProperty.Visibility = propertyModel.Visibility;
            newProperty.Description = propertyModel.Description;
            newProperty.Presentation = propertyModel.Presentation;
            return newProperty;
        }

        #endregion Public Properties
    }
}
