using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlRepositoryBase
    {

        #region Protected Fields

        protected XmlDatabaseMan context;

        #endregion Protected Fields

        #region Protected Constructors

        protected XmlRepositoryBase(XmlDatabaseMan context)
        {
            this.context = context;
        }

        #endregion Protected Constructors

        #region Protected Methods

        protected IEntry Create(Type type)
        {
            return Activator.CreateInstance(type) as IEntry;
        }

        #endregion Protected Methods

    }
}