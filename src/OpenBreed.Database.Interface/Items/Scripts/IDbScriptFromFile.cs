using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Scripts
{
    public interface IDbScriptFromFile : IDbScript
    {
        #region Public Properties

        string DataRef { get; set; }

        #endregion Public Properties
    }
}