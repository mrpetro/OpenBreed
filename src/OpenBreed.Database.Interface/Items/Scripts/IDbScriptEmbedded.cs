using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Scripts
{
    public interface IDbScriptEmbedded : IDbScript
    {
        #region Public Properties

        string Script { get; set; }

        #endregion Public Properties
    }
}