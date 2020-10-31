using OpenBreed.Common;
using OpenBreed.Database.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    internal static class DbEditorVMHelper
    {
        internal static bool TrySaveDatabase(DbEditorVM dbEditor)
        {
            if (dbEditor.Editable == null)
                throw new InvalidOperationException("Expected current database");

            dbEditor.Save();
            return true;
        }
    }
}
