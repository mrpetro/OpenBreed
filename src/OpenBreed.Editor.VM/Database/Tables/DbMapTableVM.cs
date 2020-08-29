﻿using OpenBreed.Common;
using OpenBreed.Common.Maps;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Editor.VM.Database.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DbMapTableVM : DbTableVM
    {
        #region Private Fields

        private readonly IRepository<IMapEntry> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DbMapTableVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Maps"; } }

        #endregion Public Properties

    }
}
