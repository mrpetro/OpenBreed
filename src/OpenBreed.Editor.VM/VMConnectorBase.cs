using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM
{
    public class VMConnectorBase<T>
    {
        #region Public Constructors

        public VMConnectorBase(T source)
        {
            Source = source;
        }

        #endregion Public Constructors

        #region Internal Properties

        internal T Source { get; }

        #endregion Internal Properties
    }
}
