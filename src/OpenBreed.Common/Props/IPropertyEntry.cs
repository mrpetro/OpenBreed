using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Props
{
    public interface IPropertyEntry
    {
        #region Public Properties

        string Color { get; }
        string Description { get; }
        int Id { get; }
        string ImagePath { get; }
        string Name { get; }
        bool Visibility { get; }

        #endregion Public Properties
    }
}
