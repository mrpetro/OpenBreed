using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Texts
{
    public interface IDbTextEmbedded : IDbText
    {
        #region Public Properties

        string Text { get; set; }

        #endregion Public Properties
    }
}