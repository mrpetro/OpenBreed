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

        string Description { get; }
        int Id { get; }
        string Name { get; }

        IPropertyPresentation Presentation { get; }
        IPropertyTriggers Triggers { get; }

        #endregion Public Properties
    }
}
