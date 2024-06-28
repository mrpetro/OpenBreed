using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Texts
{
    public interface IDbTextFromMap : IDbText
    {
        #region Public Properties

        string DataRef { get; set; }

        string BlockName { get; set; }

        #endregion Public Properties
    }
}