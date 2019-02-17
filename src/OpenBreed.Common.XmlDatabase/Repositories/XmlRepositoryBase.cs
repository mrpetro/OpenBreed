using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBreed.Common.XmlDatabase.Repositories
{
    public class XmlRepositoryBase
    {
        #region Protected Fields

        protected XmlDatabase context;

        #endregion Protected Fields

        #region Protected Constructors

        protected XmlRepositoryBase(XmlDatabase context)
        {
            this.context = context;
        }

        #endregion Protected Constructors

    }
}