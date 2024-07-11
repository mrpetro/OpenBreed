using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Tools
{
    public interface IScrollableVM
    {
        #region Public Methods

        void ScrollViewBy(int deltaX, int deltaY);

        #endregion Public Methods
    }
}
