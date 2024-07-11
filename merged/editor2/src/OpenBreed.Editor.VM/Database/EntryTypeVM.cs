using OpenBreed.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    public class EntryTypeVM
    {

        #region Public Constructors

        public EntryTypeVM(Type type)
        {
            Type = type;
            Name = ProgramTools.GetAttributeValue<DescriptionAttribute>(type, a => a.Description);
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get; }

        #endregion Public Properties

        #region Internal Properties

        internal Type Type { get; }

        #endregion Internal Properties

    }
}
