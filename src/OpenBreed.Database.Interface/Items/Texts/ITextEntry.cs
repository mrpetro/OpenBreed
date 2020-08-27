using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Texts
{
    public interface ITextEntry : IEntry
    {
        #region Public Properties

        string DataRef { get; set; }

        #endregion Public Properties
    }
}
