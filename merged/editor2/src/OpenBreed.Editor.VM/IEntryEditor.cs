using OpenBreed.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM
{
    public interface IEntryEditor<E> : INotifyPropertyChanged
    {
        void UpdateEntry(E entry);
        void UpdateVM(E entry);
    }
}
