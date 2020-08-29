using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Interface.Items
{
    public interface IEntry
    {

        #region Public Properties

        string Id { get; set; }

        string Description { get; set; }

        #endregion Public Properties

        #region Public Methods

        IEntry Copy();

        #endregion Public Methods
    }
}
