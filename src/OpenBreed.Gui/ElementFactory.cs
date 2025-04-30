using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui
{
    internal class ElementFactory : IElementFactory
    {
        public TElement Create<TElement>(string blueprintName, object model) where TElement : IElement
        {
            throw new NotImplementedException();
        }
    }
}
