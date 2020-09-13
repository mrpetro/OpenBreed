using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM
{
    public class EditableEntryVM : BaseViewModel
    {
        #region Private Fields

        private string _id;
        private string _description;

        #endregion Private Fields

        #region Public Properties


        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        internal virtual void FromEntry(IEntry entry)
        {
            Id = entry.Id;
            Description = entry.Description;
        }

        internal virtual void ToEntry(IEntry entry)
        {
            entry.Id = Id;
            entry.Description = Description;
        }

        #endregion Public Properties
    }
}
