using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.Database.Tables
{
    public class DatabaseTableDef : DatabaseItemDef
    {
        #region Public Methods

        public override string ToString()
        {
            return GetType().ToString();
        }

        #endregion Public Methods
    }
}
