using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Base
{
    public static class BindingListExtensions
    {
        public static BindingList<T> UpdateAfter<T>(this BindingList<T> list, Action action)
        {
            var state = list.RaiseListChangedEvents;

            try
            {
                list.RaiseListChangedEvents = false;
                action();
            }
            finally
            {
                list.RaiseListChangedEvents = true;
                list.ResetBindings();
            }

            return list;
        }
    }
}
