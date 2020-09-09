using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.EntityTemplates
{
    public interface IEntityTemplateEntry : IEntry
    {
        #region Public Properties

        string DataRef { get; set; }

        #endregion Public Properties
    }
}
