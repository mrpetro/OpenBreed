using OpenBreed.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM
{
    public interface IEntryEditor<E, VM>
    {
        VM CreateVM(E entry);
        void UpdateEntry(VM source, E entry);
        void UpdateVM(E entry, VM target);
    }
}
