using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Builders
{
    public interface ICheckboxBuilder : IElementBuilder
    {
        void SetChecked(bool isChecked);

    }
}
