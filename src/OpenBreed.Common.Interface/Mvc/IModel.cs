using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Mvc
{
    public interface IModel
    {
    }

    public interface IEditorModel : IModel
    {
        #region Public Properties

        //bool IsModified { get; }

        #endregion Public Properties
    }
}